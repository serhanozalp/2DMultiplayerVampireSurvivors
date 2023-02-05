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
        public string RelayCode;
        public Dictionary<Type, BaseGameMode> GameModes;
    }

    private LobbyData _localLobbyData;

    public string RelayCode { get => _localLobbyData.RelayCode; set { _localLobbyData.RelayCode = value; } }

    public void ApplyLobbyData(Lobby lobby)
    {
        _localLobbyData = new LobbyData(){ 
            LobbyName = lobby.Name, 
            GameModes = new Dictionary<Type, BaseGameMode>(), 
            RelayCode = lobby.Data[ConstantDictionary.KEY_LOBBY_OPTIONS_RELAYCODE].Value };
        var gameModeData = lobby.Data.Where(kvp => Type.GetType(kvp.Key)?.BaseType == typeof(BaseGameMode)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        foreach (var pair in gameModeData)
        {
            Type type = Type.GetType(pair.Key);
            var gameMode = GameModeDataSource.GetGameModeByTypeAndModeName(type, pair.Value.Value);
            _localLobbyData.GameModes.Add(type, gameMode);
        }
    }
}
