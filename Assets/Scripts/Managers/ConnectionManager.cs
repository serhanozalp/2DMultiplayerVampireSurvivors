using Abstracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;

public class ConnectionManager : BaseConnectionManager
{
    public ConnectionManager() : base()
    {
    }

    public override async void StartClientAsync(Lobby lobby)
    {
        if (!await StartClientSequenceAsync(lobby)) _connectionEventMessageChannel.Publish(ConnectionEventMessage.StartingClientFailed);
        else _connectionEventMessageChannel.Publish(ConnectionEventMessage.Connected);
    }

    public override async void StartHostAsync(string lobbyName, Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        if (!await StartHostSequenceAsync(lobbyName, selectedGameModeNameDictionary)) _connectionEventMessageChannel.Publish(ConnectionEventMessage.StartingHostFailed);
        else _connectionEventMessageChannel.Publish(ConnectionEventMessage.Connected);
    }

    public override async void ShutdownAsync(bool forceQuit = false)
    {
        if (!forceQuit) _connectionEventMessageChannel.Publish(ConnectionEventMessage.StartedShutdown);
        if (!await ShutdownSequenceAsync(forceQuit) && !forceQuit) _connectionEventMessageChannel.Publish(ConnectionEventMessage.ShutdownFailed);
        else if (!forceQuit) _connectionEventMessageChannel.Publish(ConnectionEventMessage.ShutdownComplete);
    }

    private async Task<bool> StartHostSequenceAsync(string lobbyName, Dictionary<Type, string> selectedGameModeNameDictionary)
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

    private async Task<bool> StartClientSequenceAsync(Lobby lobby)
    {
        if (!await _authenticationServiceFacade.TryAuthorizePlayerAsync()) return false;
        var joinRequestResult = !String.IsNullOrEmpty(lobby.LobbyCode) ? await _lobbyServiceFacade.TryJoinLobbyByCodeAsync(lobby.LobbyCode) : await _lobbyServiceFacade.TryJoinLobbyByIdAsync(lobby.Id);
        if (!joinRequestResult.isSuccessful) return false;
        if (!await _relayServiceFacade.TryJoinAllocationAsync(lobby.Data[ConstantDictionary.KEY_LOBBY_OPTIONS_RELAYCODE].Value)) return false;
        _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateStartingClient);
        if (!await AsyncTaskUtils.WaitUntil(() => { return _networkManager.IsConnectedClient; }, ConstantDictionary.NETWORK_CHECK_INTERVAL, ConstantDictionary.NETWORK_TIMEOUT_DURATION))
        {
            return false;
        }
        _localLobby.SetLobbyData(joinRequestResult.joinedLobby);
        return true;
    }

    private async Task<bool> ShutdownSequenceAsync(bool forceQuit=false)
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
        if(!forceQuit) _networkConnectionStateMachine.RequestShutdown();
        _localLobby.Reset();
        return true;
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

    public override void QueryLobbies(Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        throw new NotImplementedException();
    }
}
