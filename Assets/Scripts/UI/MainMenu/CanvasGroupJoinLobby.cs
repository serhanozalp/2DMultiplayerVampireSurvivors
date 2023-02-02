using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        await _mainMenuMediator.QuerryLobbiesAsync(new Dictionary<Type, string>());
        Unblock();
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
