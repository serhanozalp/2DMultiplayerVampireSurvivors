using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;

public class CanvasGroupJoinLobby : BaseCanvasGroup
{
    [SerializeField]
    private Button _buttonResfreshLobbies;
    [SerializeField]
    private Transform _lobbyListContainer;
    [SerializeField]
    private Dropdown _prefabDropDown;
    [SerializeField]
    private MainMenuMediator _mainMenuMediator;
    [SerializeField]
    private Transform _dropDownContainer;
    [SerializeField]
    private LobbyListItemUI _prefabLobbyListItemUI;
    [SerializeField]
    private Toggle _toggleUseFilters;

    private List<Dropdown> _dropDownList;

    protected override void Awake()
    {
        base.Awake();
        _dropDownList = new List<Dropdown>();
        _buttonResfreshLobbies.onClick.AddListener(ButtonRefreshLobbiesClickedAsync);
    }

    private void Start()
    {
        SetupGameModeDropDowns();
    }

    private void SetupGameModeDropDowns()
    {
        foreach (var pair in GameModeDataSource.GameModeNameDictionary)
        {
            var dropDown = Instantiate(_prefabDropDown, _dropDownContainer);
            dropDown.name = pair.Key.ToString();
            dropDown.AddOptions(pair.Value);
            _dropDownList.Add(dropDown);
        }
    }

    private async void ButtonRefreshLobbiesClickedAsync()
    {
        Block();
        Dictionary<Type, string> selectedGameModeNameDictionary = new Dictionary<Type, string>();
        if (_toggleUseFilters.isOn)
        {
            foreach (var dropDown in _dropDownList)
            {
                selectedGameModeNameDictionary.Add(Type.GetType(dropDown.name), dropDown.options[dropDown.value].text);
            }
        }
        var queriedLobbies = await _mainMenuMediator.QuerryLobbiesAsync(selectedGameModeNameDictionary);
        SetupLobbyListUI(queriedLobbies);
        Unblock();
    }

    private void ClearLobbyListUI()
    {
        for (int i = 0; i < _lobbyListContainer.childCount; i++)
        {
            Destroy(_lobbyListContainer.GetChild(i).gameObject);
        }
    }

    private void SetupLobbyListUI(List<Lobby> queriedLobbies)
    {
        ClearLobbyListUI();
        foreach (var lobby in queriedLobbies ?? new List<Lobby>())
        {
            AddLobbyListItemUI();
        }
    }

    private void AddLobbyListItemUI()
    {
        var lobbyListItemUI = Instantiate(_prefabLobbyListItemUI, _lobbyListContainer);
        // Setup LobbyListItemUI
    }

    public override void Hide()
    {
        _myCanvasGroup.alpha = 0f;
        _myCanvasGroup.blocksRaycasts = false;
    }

    public override void Show()
    {
        _myCanvasGroup.alpha = 1f;
        _myCanvasGroup.blocksRaycasts = true;
    }
}
