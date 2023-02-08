using Abstracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;

public class ConnectionManager 
{
    private readonly BaseLobbyServiceFacade _lobbyServiceFacade;
    private readonly BaseAuthenticationServiceFacade _authenticationServiceFacade;
    private readonly BaseRelayServiceFacade _relayServiceFacade;
    private readonly NetworkConnectionStateMachine _networkConnectionStateMachine;
    private readonly NetworkManager _networkManager;

    public ConnectionManager()
    {
        _lobbyServiceFacade = ServiceLocator.Instance.GetService<LobbyServiceFacade>(true);
        _authenticationServiceFacade = ServiceLocator.Instance.GetService<AuthenticationServiceFacade>(true);
        _relayServiceFacade = ServiceLocator.Instance.GetService<RelayServiceFacade>(true);
        _networkConnectionStateMachine = ServiceLocator.Instance.GetService<NetworkConnectionStateMachine>(true);
        _networkManager = ServiceLocator.Instance.GetService<NetworkManager>(true);
    }

    public async Task<bool> JoinLobbyAsync(Lobby lobby)
    {
        if (!await _authenticationServiceFacade.TryAuthorizePlayerAsync()) return false;
        if (!await _relayServiceFacade.TryJoinAllocationAsync(lobby.Data[ConstantDictionary.KEY_LOBBY_OPTIONS_RELAYCODE].Value)) return false;
        _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateStartingClient);
        if (!await AsyncTaskUtils.WaitUntil(() => { return _networkManager.IsConnectedClient; }, ConstantDictionary.NETWORK_ISCONNECTED_CHECK_INTERVAL, ConstantDictionary.NETWORK_ISCONNECTED_WAIT_TIME))
        {
            _networkConnectionStateMachine.RequestShutdown();
            return false;
        }
        if (!await _lobbyServiceFacade.TryJoinLobbyByIdAsync(lobby.Id))
        {
            _networkConnectionStateMachine.RequestShutdown();
            return false;
        }
        
        return true;
    }

    public async Task<bool> CreateLobbyAsync(string lobbyName, Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        if (!await _authenticationServiceFacade.TryAuthorizePlayerAsync()) return false;
        if(!await _relayServiceFacade.TryCreateAllocationAsync()) return false;
        _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateStartingHost);
        if (!_networkManager.IsListening) return false;
        if (!await _lobbyServiceFacade.TryCreateLobbyAsync(lobbyName, selectedGameModeNameDictionary))
        {
            _networkConnectionStateMachine.RequestShutdown();
            return false;
        }
        return true;
    }

    public async Task<List<Lobby>> QueryLobbiesAsync(Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        if (!await _authenticationServiceFacade.TryAuthorizePlayerAsync()) return null;
        var queriedLobbies = await _lobbyServiceFacade.TryQueryLobbiesAsync(selectedGameModeNameDictionary);
        return queriedLobbies;
    }
}
