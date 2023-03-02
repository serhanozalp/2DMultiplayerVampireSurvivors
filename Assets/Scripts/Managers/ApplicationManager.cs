using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using Abstracts;
using System.Collections;

public class ApplicationManager : MonoBehaviour
{
    private BaseConnectionManager _connectionManager;
    private LocalLobby _localLobby;

    private void Awake()
    {
        ServiceLocator.Instance.RegisterService(new MessageChannel<ConnectionEventMessage>());
        ServiceLocator.Instance.RegisterService(new MessageChannel<QueriedLobbyListMessage>());
        ServiceLocator.Instance.RegisterService(new LocalLobby());
        ServiceLocator.Instance.RegisterService(new ProfileManagerPlayerPrefs());
        ServiceLocator.Instance.RegisterService(new RelayServiceFacade());
        ServiceLocator.Instance.RegisterService(new AuthenticationServiceFacade());
        ServiceLocator.Instance.RegisterService(new LobbyServiceFacade());
        ServiceLocator.Instance.RegisterService(new SessionManager()); // We are creating this for both client and host. We should change this by creating only for host.
        ServiceLocator.Instance.RegisterService(new NetworkConnectionStateMachine());
        ServiceLocator.Instance.RegisterService(new ConnectionManagerCommandPattern());

        DontDestroyOnLoad(this);
        _connectionManager = ServiceLocator.Instance.GetService<ConnectionManagerCommandPattern>();
        _localLobby = ServiceLocator.Instance.GetService<LocalLobby>();
    }
    private void Start()
    {
        SceneManager.LoadSceneAsync(ConstantDictionary.SCENE_NAME_MAINMENU);
    }

    private void OnEnable()
    {
        Application.wantsToQuit += Application_OnWantsToQuit;
    }

    private void OnDisable()
    {
        Application.wantsToQuit -= Application_OnWantsToQuit;
    }

    private bool Application_OnWantsToQuit()
    {
        var canQuit = !_localLobby.IsActive;
        if (!canQuit)
        {
            StartCoroutine(LeaveLobbyBeforeQuit());
        }
        return canQuit;
    }

    private IEnumerator LeaveLobbyBeforeQuit()
    {
        _connectionManager.QuitLobby();
        yield return new WaitUntil(() => { return !_localLobby.IsActive; });
        QuitApplication();
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
