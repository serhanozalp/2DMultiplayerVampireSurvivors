using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ApplicationManager : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Instance.RegisterService(new ProfileManagerPlayerPrefs());
        ServiceLocator.Instance.RegisterService(new AuthenticationServiceFacade());
        ServiceLocator.Instance.RegisterService(new LobbyServiceFacade());

        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        SceneManager.LoadSceneAsync(ConstantDictionary.SCENE_NAME_MAINMENU);
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
