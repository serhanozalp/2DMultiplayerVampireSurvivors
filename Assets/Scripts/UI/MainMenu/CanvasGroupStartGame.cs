using UnityEngine;
using UnityEngine.UI;
using Abstracts;

public class CanvasGroupStartGame : BaseMainMenuCanvasGroup
{
    [SerializeField]
    private Button _buttonStartGame, _buttonProfiles, _buttonQuitGame;

    protected override void Awake()
    {
        base.Awake();

        _buttonStartGame.onClick.AddListener(() => _mainMenuMediator.ShowCanvasGroupLobbyJoinCreate());
        _buttonProfiles.onClick.AddListener(() => _mainMenuMediator.ShowCanvasGroupProfiles());
        _buttonQuitGame.onClick.AddListener(() => _mainMenuMediator.QuitGame());
    }
}
