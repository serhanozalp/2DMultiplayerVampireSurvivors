using System.Collections.Generic;
using Abstracts;
using System;
using Unity.Services.Lobbies.Models;
using System.Linq;
using UnityEngine;
using Unity.Services.Authentication;

public class LocalLobby 
{
    public struct LobbyData
    {
        public string LobbyName;
        public string LobbyId;
        public string RelayCode;
        public bool IsPlayerTheHost;
        public Dictionary<Type, BaseGameMode> GameModes;
    }

    private LobbyData _localLobbyData;

    public string RelayCode { get => _localLobbyData.RelayCode; set { _localLobbyData.RelayCode = value; } }
    public string LobbyId => _localLobbyData.LobbyId;
    public bool IsPlayerTheHost => _localLobbyData.IsPlayerTheHost;

    public void SetLobbyData(Lobby lobby)
    {
        _localLobbyData = new LobbyData() {
            LobbyName = lobby.Name,
            LobbyId = lobby.Id,
            GameModes = new Dictionary<Type, BaseGameMode>(),
            IsPlayerTheHost = lobby.HostId == AuthenticationService.Instance.PlayerId,
            RelayCode = lobby.Data[ConstantDictionary.KEY_LOBBY_OPTIONS_RELAYCODE].Value };
        var gameModeData = lobby.Data.Where(kvp => Type.GetType(kvp.Key)?.BaseType == typeof(BaseGameMode)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        Debug.Log(AuthenticationService.Instance.PlayerId);
        foreach (var pair in gameModeData)
        {
            Type type = Type.GetType(pair.Key);
            var gameMode = GameModeDataSource.GetGameModeByTypeAndModeName(type, pair.Value.Value);
            _localLobbyData.GameModes.Add(type, gameMode);
        }
    }
}
