using System.Collections.Generic;
using Abstracts;
using System;
using Unity.Services.Lobbies.Models;

public class LocalLobby 
{
    public struct LobbyData
    {
        public string LobbyName;
        public Dictionary<Type, BaseGameMode> gameModes;
    }

    private LobbyData _lobbyData;

    public void ApplyLobbyData(Lobby lobby)
    {
        _lobbyData = new LobbyData { LobbyName = lobby.Name, gameModes = new Dictionary<Type, BaseGameMode>() };
        foreach (var pair in lobby.Data)
        {
            Type type = Type.GetType(pair.Key);
            var gameMode = GameModeDataSource.GetGameModeByTypeAndModeName(type, pair.Value.Value);
            _lobbyData.gameModes.Add(type, gameMode);
        }
    }
}
