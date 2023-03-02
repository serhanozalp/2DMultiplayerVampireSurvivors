using Abstracts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CanvasGroupCreateLobby : BaseCanvasGroup
{
    [SerializeField]
    private CanvasGroupLobbyJoinCreate _canvasGroupLobbyJoinCreate;
    [SerializeField]
    private Dropdown _prefabDropDown;
    [SerializeField]
    private Transform _dropDownContainer;
    [SerializeField]
    private Button _buttonCreateLobby;
    [SerializeField]
    private TMP_InputField _inputFieldLobbyName;
    [SerializeField]
    private MainMenuMediator _mainMenuMediator;

    private List<Dropdown> _dropDownList;

    protected override void Awake()
    {
        base.Awake();
        _dropDownList = new List<Dropdown>();
        _buttonCreateLobby.onClick.AddListener(ButtonCreateLobbyClicked);
    }

    private void Start()
    {
        SetupGameModeDropDowns();
    }

    private void ButtonCreateLobbyClicked()
    {
        var lobbyName = _inputFieldLobbyName.text;
        if (!IsLobbyNameValid(lobbyName))
        {
            PopupManager.Instance.AddPopup("Lobby Name Error", "Lobby Name Invalid!");
            return;
        }
        Dictionary<Type, string> selectedGameModeNameDictionary = new Dictionary<Type, string>();
        foreach(var dropDown in _dropDownList)
        {
            selectedGameModeNameDictionary.Add(Type.GetType(dropDown.name), dropDown.options[dropDown.value].text);
        }
        _mainMenuMediator.CreateLobby(lobbyName, selectedGameModeNameDictionary);
    }

    private void SetupGameModeDropDowns()
    {
        foreach(var pair in GameModeDataSource.GameModeNameDictionary)
        {
            var dropDown = Instantiate(_prefabDropDown, _dropDownContainer);
            dropDown.name = pair.Key.ToString();
            dropDown.AddOptions(pair.Value);
            _dropDownList.Add(dropDown);
        }
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

    private bool IsLobbyNameValid(string lobbyName)
    {
        return !String.IsNullOrWhiteSpace(lobbyName);
    }
}
