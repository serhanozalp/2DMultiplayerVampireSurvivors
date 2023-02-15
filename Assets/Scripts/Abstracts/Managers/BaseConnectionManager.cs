using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;

public enum ConnectionEventMessage
{
    StartedShutdown,
    Connected,
    ShutdownComplete,
    ShutdownFailed,
    StartingHostFailed,
    StartingClientFailed,
    DisconnectedHostShutdown,
    DisconnectedServerFull
}

namespace Abstracts
{
    public abstract class BaseConnectionManager
    {
        protected readonly BaseLobbyServiceFacade _lobbyServiceFacade;
        protected readonly BaseAuthenticationServiceFacade _authenticationServiceFacade;
        protected readonly BaseRelayServiceFacade _relayServiceFacade;
        protected readonly NetworkConnectionStateMachine _networkConnectionStateMachine;
        protected readonly NetworkManager _networkManager;
        protected readonly LocalLobby _localLobby;
        protected readonly BaseMessageChannel<ConnectionEventMessage> _connectionEventMessageChannel;
        protected readonly LobbyPing _lobbyPing;

        public BaseConnectionManager()
        {
            _lobbyServiceFacade = ServiceLocator.Instance.GetService<LobbyServiceFacade>(true);
            _authenticationServiceFacade = ServiceLocator.Instance.GetService<AuthenticationServiceFacade>(true);
            _relayServiceFacade = ServiceLocator.Instance.GetService<RelayServiceFacade>(true);
            _networkConnectionStateMachine = ServiceLocator.Instance.GetService<NetworkConnectionStateMachine>(true);
            _networkManager = ServiceLocator.Instance.GetService<NetworkManager>(true);
            _localLobby = ServiceLocator.Instance.GetService<LocalLobby>(true);
            _connectionEventMessageChannel = ServiceLocator.Instance.GetService<MessageChannel<ConnectionEventMessage>>(true);
            _lobbyPing = new LobbyPing(_lobbyServiceFacade);
        }

        public abstract void StartClientAsync(Lobby lobby);

        public abstract void StartHostAsync(string lobbyName, Dictionary<Type, string> selectedGameModeNameDictionary);

        public abstract void ShutdownAsync(bool forceQuit = false);

        public abstract void QueryLobbies(Dictionary<Type, string> selectedGameModeNameDictionary);
    }
}


