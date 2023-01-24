using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using Abstracts;
using System;

public class MainMenuMediator : MonoBehaviour
{
    [SerializeField]
    private BaseMainMenuCanvasGroup _canvasGroupStartGame, _canvasGroupProfiles, _canvasGroupLobbyList;
    [SerializeField]
    private GameObject _loadingSpinner;

    private BaseMainMenuCanvasGroup _currentCanvasGroup;
    private AuthenticationServiceFacade _authenticationServiceFacade;
    private BaseProfileManager _profileManager;
    private ApplicationManager _applicationManager;

    private void Awake()
    {
        _authenticationServiceFacade = ServiceLocator.Instance.GetService<AuthenticationServiceFacade>();
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

    public async Task ShowCanvasGroupLobbyListAsync()
    {
        if (!await _authenticationServiceFacade.EnsurePlayerIsAuthorizedAsync()) return;
        _currentCanvasGroup?.Hide();
        _canvasGroupLobbyList.Show();
        _currentCanvasGroup = _canvasGroupLobbyList;
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

    #region Authentication
    public void SignOutFromUnityServices()
    {
        _authenticationServiceFacade.TrySignOut();
        ShowCanvasGroupStartGame();
    }
    #endregion

    public void QuitGame()
    {
        _applicationManager.QuitApplication();
    }
}
