using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

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
        protected readonly LobbyPing _lobbyPing;

        public BaseConnectionManager()
        {
            _lobbyServiceFacade = ServiceLocator.Instance.GetService<LobbyServiceFacade>(true);
            _authenticationServiceFacade = ServiceLocator.Instance.GetService<AuthenticationServiceFacade>(true);
            _relayServiceFacade = ServiceLocator.Instance.GetService<RelayServiceFacade>(true);
            _networkConnectionStateMachine = ServiceLocator.Instance.GetService<NetworkConnectionStateMachine>(true);
            _networkManager = ServiceLocator.Instance.GetService<NetworkManager>(true);
            _localLobby = ServiceLocator.Instance.GetService<LocalLobby>(true);
            _lobbyPing = new LobbyPing(_lobbyServiceFacade);
        }

        public abstract Task<bool> QuitLobbyAsync(bool forceQuit = false);

        public abstract Task<bool> JoinLobbyAsync(Lobby lobby);

        public abstract Task<bool> CreateLobbyAsync(string lobbyName, Dictionary<Type, string> selectedGameModeNameDictionary);

        public abstract Task<List<Lobby>> QueryLobbiesAsync(Dictionary<Type, string> selectedGameModeNameDictionary);
    }
}


