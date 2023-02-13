using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;

namespace Abstracts
{
    public abstract class BaseLobbyServiceFacade 
    {
        public abstract Task<(bool isSuccessful, Lobby createdLobby)> TryCreateLobbyAsync(string lobbyName, CreateLobbyOptions createLobbyOptions);

        public abstract Task<List<Lobby>> TryQueryLobbiesAsync(QueryLobbiesOptions queryLobbiesOptions);

        public abstract Task<(bool isSuccessful, Lobby joinedLobby)> TryJoinLobbyByIdAsync(string lobbyId);

        public abstract Task<(bool isSuccessful, Lobby joinedLobby)> TryJoinLobbyByCodeAsync(string lobbyCode);

        public abstract Task<bool> TryDeleteLobbyAsync(string lobbyId);

        public abstract Task<bool> TryRemovePlayerAsync(string lobbyId, string playerId);

        public abstract void TrySendHeartBeatPingAsync(string lobbyId);

        protected bool IsLobbyNameValid(string lobbyName)
        {
            return String.IsNullOrWhiteSpace(lobbyName) ? false : true;
        }
    }
}

