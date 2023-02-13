using Abstracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Extensions;

public class ConnectionManager : BaseConnectionManager
{
    public ConnectionManager() : base()
    {
    }

    public override async Task<bool> QuitLobbyAsync(bool forceQuit=false)
    {
        if (!await _authenticationServiceFacade.TryAuthorizePlayerAsync() && !forceQuit) return false;
        if (_localLobby.IsPlayerTheHost)
        {
            if (!await _lobbyServiceFacade.TryDeleteLobbyAsync(_localLobby.LobbyId) && !forceQuit) return false;
            _lobbyPing.StopPing();
        }
        else
        {
            if (!await _lobbyServiceFacade.TryRemovePlayerAsync(_localLobby.LobbyId, AuthenticationService.Instance.PlayerId) && !forceQuit) return false;
        }
        _networkConnectionStateMachine.RequestShutdown();
        _localLobby.Reset();
        return true;
    }

    public override async Task<bool> JoinLobbyAsync(Lobby lobby)
    {
        if (!await _authenticationServiceFacade.TryAuthorizePlayerAsync()) return false;
        var joinRequestResult = !String.IsNullOrEmpty(lobby.LobbyCode) ? await _lobbyServiceFacade.TryJoinLobbyByCodeAsync(lobby.LobbyCode) : await _lobbyServiceFacade.TryJoinLobbyByIdAsync(lobby.Id);
        if (!joinRequestResult.isSuccessful) return false;
        _localLobby.SetLobbyData(joinRequestResult.joinedLobby);
        if (!await _relayServiceFacade.TryJoinAllocationAsync(lobby.Data[ConstantDictionary.KEY_LOBBY_OPTIONS_RELAYCODE].Value)) return false;
        _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateStartingClient);
        if (!await AsyncTaskUtils.WaitUntil(() => { return _networkManager.IsConnectedClient; }, ConstantDictionary.NETWORK_ISCONNECTED_CHECK_INTERVAL, ConstantDictionary.NETWORK_ISCONNECTED_WAIT_TIME))
        {
            _networkConnectionStateMachine.RequestShutdown();
            await QuitLobbyAsync();
            return false;
        }
        return true;
    }

    public override async Task<bool> CreateLobbyAsync(string lobbyName, Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        if (!await _authenticationServiceFacade.TryAuthorizePlayerAsync()) return false;
        var allocateRequestResult = await _relayServiceFacade.TryCreateAllocationAsync();
        if (!allocateRequestResult.isSuccessful) return false;
        _localLobby.RelayCode = allocateRequestResult.relayCode;
        _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateStartingHost);
        if (!_networkManager.IsListening) return false;
        var createRequestResult = await _lobbyServiceFacade.TryCreateLobbyAsync(lobbyName, GenerateCreateLobbyOptions(_localLobby.RelayCode, selectedGameModeNameDictionary));
        if (!createRequestResult.isSuccessful)
        {
            _networkConnectionStateMachine.RequestShutdown();
            return false;
        }
        var createdLobby = createRequestResult.createdLobby;
        _localLobby.SetLobbyData(createdLobby);
        _lobbyPing.StartPing(createdLobby.Id);
        return true;
    }

    public override async Task<List<Lobby>> QueryLobbiesAsync(Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        if (!await _authenticationServiceFacade.TryAuthorizePlayerAsync()) return null;
        var queriedLobbies = await _lobbyServiceFacade.TryQueryLobbiesAsync(GenerateQueryLobbiesOptions(selectedGameModeNameDictionary));
        return queriedLobbies;
    }

    private QueryLobbiesOptions GenerateQueryLobbiesOptions(Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
        {
            Filters = new List<QueryFilter>()
        };
        foreach (var pair in selectedGameModeNameDictionary)
        {
            Type type = pair.Key;
            queryLobbiesOptions.Filters.Add(new QueryFilter(
                GameModeDataSource.GetGameModeByTypeAndModeName(type, pair.Value).DataObjectIndexOptions.ToQueryFilterFieldOptions(),
                pair.Value,
                QueryFilter.OpOptions.EQ));
        }
        return queryLobbiesOptions;
    }

    private CreateLobbyOptions GenerateCreateLobbyOptions(string relayCode, Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
        {
            IsPrivate = false,
            Data = new Dictionary<string, DataObject>()
        };
        foreach (var pair in selectedGameModeNameDictionary)
        {
            Type type = pair.Key;
            createLobbyOptions.Data.Add(pair.Key.ToString(), new DataObject(
                DataObject.VisibilityOptions.Public,
                pair.Value,
                GameModeDataSource.GetGameModeByTypeAndModeName(type, pair.Value).DataObjectIndexOptions));
        }
        createLobbyOptions.Data.Add(ConstantDictionary.KEY_LOBBY_OPTIONS_RELAYCODE, new DataObject(
            DataObject.VisibilityOptions.Public, relayCode ?? ""));
        return createLobbyOptions;
    }
}
