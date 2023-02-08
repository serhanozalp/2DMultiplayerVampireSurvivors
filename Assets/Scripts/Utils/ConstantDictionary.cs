using DG.Tweening;
public static class ConstantDictionary
{
    public const int GAMEPLAY_MAX_PLAYERS = 4;
    public const string KEY_CLIENTPREFS_PROFILE_LIST = "AvailableProfileNames";
    public const string KEY_CLIENTPREFS_PROFILE_NAME_CURRENT = "CurrentProfileName";
    public const string KEY_LOBBY_OPTIONS_RELAYCODE = "RelayCode";
    public const string SCENE_NAME_MAINMENU = "MainMenu";
    public const string RELAYSERVICE_CONNECTION_TYPE = "dtls";

    public static class PopupConstantDictionary
    {
        public static readonly int POPUPMANAGER_MAX_POPUP_ADD = 3;
        public static readonly int POPUPMANAGER_MAX_POPUP_SHOW = 1;
        public static readonly float POPUPMANAGER_MAX_POPUP_SPACING = 5f;
        public static readonly float POPUP_TWEEN_DURATION_SCALE = 0.6f;
        public static readonly float POPUP_TWEEN_DURATION_FADE = 0.5f;
        public static readonly float POPUP_TWEEN_DURATION_MOVE = 0.9f;
        public static readonly Ease POPUP_TWEEN_EASETYPE_SCALE = Ease.InQuart;
        public static readonly Ease POPUP_TWEEN_EASETYPE_FADE = Ease.Unset;
        public static readonly Ease POPUP_TWEEN_EASETYPE_MOVE = Ease.Unset;
        public static readonly int POPUP_LIFE_DURATION = 7;
    }

    public static readonly string CLIENTPREFS_PROFILE_NAME_DEFAULT = "NONAME";   
    public static readonly string PROFILE_MANAGER_REGEX_PATTERN = "^[a-zA-Z0-9_-]{3,30}$";
    public static readonly float MAINMENU_TWEEN_DURATION_MOVE = 1f;
    public static readonly Ease MAINMENU_TWEEN_EASETYPE_MOVE = Ease.OutElastic;
    public static readonly int NETWORK_ISCONNECTED_WAIT_TIME = 7000;
    public static readonly int NETWORK_ISCONNECTED_CHECK_INTERVAL = 25;
    public static readonly int LOBBYSERVICE_RATE_LIMIT_HEARTBEAT = 15000;
    public static readonly int LOBBYSERVICE_RATE_LIMIT_GET = 1000;
}
