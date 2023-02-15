using Abstracts;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGroupLobbyRoom : BaseMainMenuCanvasGroup
{
    [SerializeField]
    private Button _buttonQuitLobby;
    [SerializeField]
    private PlayerNetworkObjectList _playerNetworkObjectList;

    protected override void Awake()
    {
        base.Awake();
        _buttonQuitLobby.onClick.AddListener(() => ButtonQuitLobbyClicked());
    }

    private void ButtonQuitLobbyClicked()
    {
        _mainMenuMediator.QuitLobby();
    }
}
