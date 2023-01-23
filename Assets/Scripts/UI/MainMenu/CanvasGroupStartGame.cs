using UnityEngine;
using UnityEngine.UI;
using Abstracts;

public class CanvasGroupStartGame : BaseMainMenuCanvasGroup
{
    [SerializeField]
    private Button _buttonStartGame, _buttonProfiles, _buttonQuitGame;
    [SerializeField]
    private GameObject _loadingSpinner;

    protected override void Awake()
    {
        base.Awake();

        _buttonStartGame.onClick.AddListener(() => ButtonStartGameClickedAsync());
        _buttonProfiles.onClick.AddListener(() => _mainMenuMediator.ShowCanvasGroupProfiles());
        _buttonQuitGame.onClick.AddListener(() => _mainMenuMediator.QuitGame());
    }

    private async void ButtonStartGameClickedAsync()
    {
        Block();
        _loadingSpinner.SetActive(true);
        await _mainMenuMediator.ShowCanvasGroupLobbyListAsync();
        Unblock();
        _loadingSpinner.SetActive(false);
    }
}
