using UnityEngine;
using System.Collections.Generic;
using Abstracts;
using System;
using Unity.Services.Lobbies.Models;

public class MainMenuMediator : MonoBehaviour
{
    [SerializeField]
    private BaseMainMenuCanvasGroup _canvasGroupStartGame, _canvasGroupProfiles, _canvasGroupLobbyJoinCreate, _canvasGroupLobbyRoom;
    [SerializeField]
    private GameObject _loadingSpinner;

    private BaseMainMenuCanvasGroup _currentCanvasGroup;
    private BaseProfileManager _profileManager;
    private ApplicationManager _applicationManager;
    private BaseConnectionManager _connectionManager;
    private BaseMessageChannel<ConnectionEventMessage> _connectionEventMessageChannel;
    private BaseMessageChannel<QueriedLobbyListMessage> _queriedLobbyListMessageChannel;

    private void Awake()
    {
        _profileManager = ServiceLocator.Instance.GetService<ProfileManagerPlayerPrefs>();
        _applicationManager = ServiceLocator.Instance.GetService<ApplicationManager>();
        _connectionManager = ServiceLocator.Instance.GetService<ConnectionManagerCommandPattern>();
        _connectionEventMessageChannel = ServiceLocator.Instance.GetService<MessageChannel<ConnectionEventMessage>>();
        _queriedLobbyListMessageChannel = ServiceLocator.Instance.GetService<MessageChannel<QueriedLobbyListMessage>>();
    }

    private void OnEnable()
    {
        _connectionEventMessageChannel.Subscribe(connectionEventMessage => HandleConnectionEventMessage(connectionEventMessage));
        _queriedLobbyListMessageChannel.Subscribe(queriedLobbyListMessage => HandleQueriedLobbyListMessage(queriedLobbyListMessage));
    }

    private void Start()
    {
        ShowCanvasGroupStartGame();
    }

    private void OnDisable()
    {
        _connectionEventMessageChannel.Unsubscribe(connectionEventMessage => HandleConnectionEventMessage(connectionEventMessage));
        _queriedLobbyListMessageChannel.Unsubscribe(queriedLobbyListMessage => HandleQueriedLobbyListMessage(queriedLobbyListMessage));
    }

    #region Menu Show/Hide
    public void ShowCanvasGroupProfiles()
    {
        _currentCanvasGroup?.Hide();
        _canvasGroupProfiles.Show();
        _currentCanvasGroup = _canvasGroupProfiles;
    }
    public void ShowCanvasGroupStartGame()
    {
        _currentCanvasGroup?.Hide();
        _canvasGroupStartGame.Show();
        _currentCanvasGroup = _canvasGroupStartGame;
    }

    public void ShowCanvasGroupLobbyJoinCreate()
    {
        _currentCanvasGroup?.Hide();
        _canvasGroupLobbyJoinCreate.Show();
        _currentCanvasGroup = _canvasGroupLobbyJoinCreate;
    }

    public void ShowCanvasGroupLobbyRoom()
    {
        _currentCanvasGroup?.Hide();
        _canvasGroupLobbyRoom.Show();
        _currentCanvasGroup = _canvasGroupLobbyRoom;
    }
    #endregion

    #region ProfileManager
    public void ChangeProfileName(string profileName)
    {
        _profileManager.CurrentProfileName = profileName;
    }

    public void AddProfile(string profileName)
    {
        _profileManager.AddProfile(profileName);
    }

    public void DeleteProfile(string profileName)
    {
        _profileManager.DeleteProfile(profileName);
    }

    public List<string> GetProfileList()
    {
        return _profileManager.ProfileList;
    }
    #endregion

    #region ConnectionManager
    public void CreateLobby(string lobbyName, Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        ConfigureUiForAsyncOperations(true);
        _connectionManager.StartHostAsync(lobbyName, selectedGameModeNameDictionary);
    }

    public void JoinLobby(Lobby lobby)
    {
        ConfigureUiForAsyncOperations(true);
        _connectionManager.StartClientAsync(lobby);
    }

    public void QueryLobbies(Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        ConfigureUiForAsyncOperations(true);
        _connectionManager.QueryLobbies(selectedGameModeNameDictionary);
    }

    public void QuitLobby()
    {
        ConfigureUiForAsyncOperations(true);
        _connectionManager.QuitLobbyAndShutdownNetworkAsync();
    }

    private void HandleConnectionEventMessage(ConnectionEventMessage connectionEventMessage)
    {
        switch (connectionEventMessage)
        {
            case ConnectionEventMessage.Connected:
                ConfigureUiForAsyncOperations(false);
                ShowCanvasGroupLobbyRoom();
                break;
            case ConnectionEventMessage.ShutdownComplete:
                ConfigureUiForAsyncOperations(false);
                ShowCanvasGroupLobbyJoinCreate();
                break;
            case ConnectionEventMessage.DisconnectedHostShutdown:
                ConfigureUiForAsyncOperations(false);
                ShowCanvasGroupLobbyJoinCreate();
                break;
            case ConnectionEventMessage.DisconnectedNoReason:
                ConfigureUiForAsyncOperations(false);
                ShowCanvasGroupLobbyJoinCreate();
                break;
            default:
                ConfigureUiForAsyncOperations(false);
                break;
        }
    }

    private void HandleQueriedLobbyListMessage(QueriedLobbyListMessage queriedLobbyListMessage)
    {
        ConfigureUiForAsyncOperations(false);
    }
    #endregion

    private void ConfigureUiForAsyncOperations(bool block)
    {
        if (block)
        {
            _loadingSpinner.SetActive(true);
            _currentCanvasGroup.Block();
        }
        else
        {
            _loadingSpinner.SetActive(false);
            _currentCanvasGroup.Unblock();
        }
    }

    public void QuitGame()
    {
        _applicationManager.QuitApplication();
    }
}
