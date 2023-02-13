using System.Collections.Generic;
using Abstracts;
using System;
using Unity.Services.Lobbies.Models;
using System.Linq;
using Unity.Services.Authentication;
using Interfaces;

public class LocalLobby : IReset
{
    public struct LobbyData
    {
        public string lobbyName;
        public string lobbyId;
        public bool isActive;
        public string relayCode;
        public bool isPlayerTheHost;
        public Dictionary<Type, BaseGameMode> gameModes;
    }

    private LobbyData _localLobbyData;

    public string RelayCode { get => _localLobbyData.relayCode; set { _localLobbyData.relayCode = value; } }
    public string LobbyId => _localLobbyData.lobbyId;
    public bool IsPlayerTheHost => _localLobbyData.isPlayerTheHost;
    public bool IsActive => _localLobbyData.isActive;

    public void SetLobbyData(Lobby lobby)
    {
        _localLobbyData = new LobbyData() {
            lobbyName = lobby.Name,
            lobbyId = lobby.Id,
            isActive = true,
            relayCode = lobby.Data[ConstantDictionary.KEY_LOBBY_OPTIONS_RELAYCODE].Value,
            isPlayerTheHost = lobby.HostId == AuthenticationService.Instance.PlayerId,
            gameModes = new Dictionary<Type, BaseGameMode>()};
        var gameModeData = lobby.Data.Where(kvp => Type.GetType(kvp.Key)?.BaseType == typeof(BaseGameMode)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        foreach (var pair in gameModeData)
        {
            Type type = Type.GetType(pair.Key);
            var gameMode = GameModeDataSource.GetGameModeByTypeAndModeName(type, pair.Value.Value);
            _localLobbyData.gameModes.Add(type, gameMode);
        }
    }

    public void Reset()
    {
        _localLobbyData = new LobbyData();
    }
}
