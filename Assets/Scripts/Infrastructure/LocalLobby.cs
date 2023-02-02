using System.Collections.Generic;
using Abstracts;
using System;
using Unity.Services.Lobbies.Models;
using System.Linq;

public class LocalLobby 
{
    public struct LobbyData
    {
        public string LobbyName;
        public Dictionary<Type, BaseGameMode> gameModes;
    }

    private LobbyData _lobbyData;

    public  LobbyData Data => _lobbyData;

    public void ApplyLobbyData(Lobby lobby)
    {
        _lobbyData = new LobbyData { LobbyName = lobby.Name, gameModes = new Dictionary<Type, BaseGameMode>() };
        foreach (var pair in lobby.Data)
        {
            Type type = Type.GetType(pair.Key);
            var gameMode = GameModeDataSource.GameModeList.OfType<BaseGameMode>().First(x => x.ModeName == pair.Value.Value);
            _lobbyData.gameModes.Add(type, gameMode);
        }
    }
}
