using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;

public struct QueriedLobbyListMessage
{
    public List<Lobby> queriedLobbyList;
}

namespace Abstracts
{
    public abstract class BaseLobbyServiceFacade 
    {
        public abstract Task<Lobby> TryCreateLobbyAsync(string lobbyName, CreateLobbyOptions createLobbyOptions);

        public abstract Task<List<Lobby>> TryQueryLobbiesAsync(QueryLobbiesOptions queryLobbiesOptions);

        public abstract Task<Lobby> TryJoinLobbyByIdAsync(string lobbyId);

        public abstract Task<Lobby> TryJoinLobbyByCodeAsync(string lobbyCode);

        public abstract Task TryDeleteLobbyAsync(string lobbyId);

        public abstract Task TryRemovePlayerAsync(string lobbyId, string playerId);

        public abstract void TrySendHeartBeatPingAsync(string lobbyId);
    }
}

