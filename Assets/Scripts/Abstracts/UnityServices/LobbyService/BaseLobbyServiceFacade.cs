using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Unity.Services.Lobbies.Models;

namespace Abstracts
{
    public abstract class BaseLobbyServiceFacade 
    {
        public abstract Task<bool> TryCreateLobbyAsync(string lobbyName, Dictionary<Type, string> selectedGameModeNameDictionary);

        public abstract Task<List<Lobby>> TryQueryLobbiesAsync(Dictionary<Type, string> selectedGameModeNameDictionary);

        public abstract Task<bool> TryJoinLobbyByIdAsync(string lobbyId);

        public abstract void TrySendHeartBeatPingAsync(string lobbyId);

        public abstract void TryGetLobbyAsync(string lobbyId);

        protected bool IsLobbyNameValid(string lobbyName)
        {
            return String.IsNullOrWhiteSpace(lobbyName) ? false : true;
        }
    }
}

