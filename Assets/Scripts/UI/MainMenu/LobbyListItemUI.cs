using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Abstracts;
using Extensions;

public class LobbyListItemUI : MonoBehaviour
{
    [SerializeField]
    private Button _buttonJoinLobby;
    [SerializeField]
    private TMP_Text _textLobbyName, _textGameModes, _textPlayerCount;

    private Action<LobbyListItemUI> _onButtonJoinLobbyClicked;
    private Lobby _lobby;

    public Lobby Lobby => _lobby;

    private void Awake()
    {
        _buttonJoinLobby.onClick.AddListener(() => _onButtonJoinLobbyClicked(this));
    }

    public void Setup(Lobby lobby, Action<LobbyListItemUI> OnButtonJoinLobbyClicked)
    {
        _lobby = lobby;
        _onButtonJoinLobbyClicked = OnButtonJoinLobbyClicked;
        _textLobbyName.text = _lobby.Name;
        _textGameModes.text = GetGameModeNamesFromLobbyData(_lobby.Data);
        _textPlayerCount.text = $"{ _lobby.Players.Count } / { _lobby.MaxPlayers }";
    }

    private string GetGameModeNamesFromLobbyData(Dictionary<string,DataObject> lobbyData)
    {
        var gameModeData = lobbyData.Where(kvp => Type.GetType(kvp.Key)?.BaseType == typeof(BaseGameMode)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        List<string> gameModeNameList = new List<string>();
        foreach (var kvp in gameModeData)
        {
            gameModeNameList.Add(gameModeData[kvp.Key].Value);
        }
        return gameModeNameList.ToStringSeperatedByComma();
    }
}
