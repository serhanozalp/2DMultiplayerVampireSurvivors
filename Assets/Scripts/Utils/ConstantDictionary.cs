using DG.Tweening;
public static class ConstantDictionary
{
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

    public static readonly string SCENE_NAME_MAINMENU = "MainMenu";

    public static readonly string KEY_CLIENTPREFS_PROFILE_LIST = "AvailableProfileNames";
    public static readonly string KEY_CLIENTPREFS_PROFILE_NAME_CURRENT = "CurrentProfileName";
    public static readonly string CLIENTPREFS_PROFILE_NAME_DEFAULT = "NONAME";   

    public static readonly string PROFILE_MANAGER_REGEX_PATTERN = "^[a-zA-Z0-9_-]{3,30}$";

    public static readonly float MAINMENU_TWEEN_DURATION_MOVE = 1f;
    public static readonly Ease MAINMENU_TWEEN_EASETYPE_MOVE = Ease.OutElastic;
}
