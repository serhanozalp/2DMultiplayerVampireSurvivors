using UnityEngine;
using System.Collections.Generic;
using Abstracts;
using System;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;

public class MainMenuMediator : MonoBehaviour
{
    [SerializeField]
    private BaseMainMenuCanvasGroup _canvasGroupStartGame, _canvasGroupProfiles, _canvasGroupLobbyJoinCreate, _canvasGroupLobbyRoom;
    [SerializeField]
    private GameObject _loadingSpinner;

    private BaseMainMenuCanvasGroup _currentCanvasGroup;
    private BaseAuthenticationServiceFacade _authenticationServiceFacade;
    private BaseLobbyServiceFacade _lobbyServiceFacade;
    private BaseProfileManager _profileManager;
    private ApplicationManager _applicationManager;

    private void Awake()
    {
        _authenticationServiceFacade = ServiceLocator.Instance.GetService<AuthenticationServiceFacade>();
        _lobbyServiceFacade = ServiceLocator.Instance.GetService<LobbyServiceFacade>();
        _profileManager = ServiceLocator.Instance.GetService<ProfileManagerPlayerPrefs>();
        _applicationManager = ServiceLocator.Instance.GetService<ApplicationManager>();
    }
    private void Start()
    {
        ShowCanvasGroupStartGame();
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
        _currentCanvasGroup = _canvasGroupStartGame;
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

    #region LobbyService
    public async Task CreateLobbyAsync(string lobbyName, Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        if (String.IsNullOrWhiteSpace(lobbyName))
        {
            PopupManager.Instance.AddPopup("Lobby Name Error", "Lobby Name Is Not Valid!");
            return;
        }
        _loadingSpinner.SetActive(true);
        if (await _authenticationServiceFacade.TryAuthorizePlayerAsync())
        {
            if (await _lobbyServiceFacade.TryCreateLobbyAsync(lobbyName, selectedGameModeNameDictionary))
            {
                ShowCanvasGroupLobbyRoom();
            }
        }
        _loadingSpinner.SetActive(false);
    }

    public async Task<List<Lobby>> QuerryLobbiesAsync(Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        _loadingSpinner.SetActive(true);
        if(await _authenticationServiceFacade.TryAuthorizePlayerAsync())
        {
            var queriedLobbies = await _lobbyServiceFacade.TryQuerryLobbiesAsync(selectedGameModeNameDictionary);
            _loadingSpinner.SetActive(false);
            return queriedLobbies;
        }
        else
        {
            _loadingSpinner.SetActive(false);
            return null;
        }
    }
    #endregion

    public void QuitGame()
    {
        _applicationManager.QuitApplication();
    }
}
