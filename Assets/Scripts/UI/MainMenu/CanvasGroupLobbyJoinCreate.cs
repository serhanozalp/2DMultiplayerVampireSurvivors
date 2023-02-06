using UnityEngine;
using UnityEngine.UI;
using Abstracts;
using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using System;

public class CanvasGroupLobbyJoinCreate : BaseMainMenuCanvasGroup
{
    [SerializeField]
    private Button _buttonBack;

    protected override void Awake()
    {
        base.Awake();
        _buttonBack.onClick.AddListener(() => _mainMenuMediator.ShowCanvasGroupStartGame());
    }

    public async Task<List<Lobby>> QueryLobbiesAsync(Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        Block();
        var queriedLobbies = await _mainMenuMediator.QueryLobbiesAsync(selectedGameModeNameDictionary);
        Unblock();
        return queriedLobbies;
    }

    public async void CreateLobbyAsync(string lobbyName, Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        Block();
        await _mainMenuMediator.CreateLobbyAsync(lobbyName, selectedGameModeNameDictionary);
        Unblock();
    }

    public async void JoinLobbyAsync(Lobby lobby)
    {
        Block();
        await _mainMenuMediator.JoinLobbyAsync(lobby);
        Unblock();
    }
}
