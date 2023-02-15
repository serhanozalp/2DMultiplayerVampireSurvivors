using UnityEngine;
using UnityEngine.UI;
using Abstracts;

public class CanvasGroupLobbyJoinCreate : BaseMainMenuCanvasGroup
{
    [SerializeField]
    private Button _buttonBack;

    protected override void Awake()
    {
        base.Awake();
        _buttonBack.onClick.AddListener(() => _mainMenuMediator.ShowCanvasGroupStartGame());
    }
}
