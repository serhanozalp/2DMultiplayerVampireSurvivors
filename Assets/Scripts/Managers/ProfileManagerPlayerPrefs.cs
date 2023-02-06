using System;
using System.Collections.Generic;
using Abstracts;

public class ProfileManagerPlayerPrefs : BaseProfileManager
{
    protected override string RegexPattern => ConstantDictionary.PROFILE_MANAGER_REGEX_PATTERN;

    public ProfileManagerPlayerPrefs() : base() { }

    public override void AddProfile(string profileName)
    {
        if (!IsProfileNameValid(profileName))
        {
            PopupManager.Instance.AddPopup("Profile Name Error", "Profile Name Is Not Valid!");
            return;
        }
        if (_profileList.Contains(profileName))
        {
            PopupManager.Instance.AddPopup("Profile Name Error", "Profile Name Already Exists!");
            return;
        }
        _profileList.Add(profileName);
        SaveProfileList();
    }

    public override void DeleteProfile(string profileName)
    {
        _profileList.Remove(profileName);
        if (profileName == _currentProfileName) CurrentProfileName = ConstantDictionary.CLIENTPREFS_PROFILE_NAME_DEFAULT;
        SaveProfileList();
    }

    protected override string LoadCurrentProfileName()
    {
#if UNITY_EDITOR
        return Guid.NewGuid().ToString().Substring(0,30);
#else
        return PlayerPrefsRepository.LoadCurrentProfileName();
#endif
    }

    protected override List<string> LoadProfileList()
    {
        return PlayerPrefsRepository.LoadProfileList();
    }

    protected override void SaveCurrentProfileName(string profileName)
    {
        PlayerPrefsRepository.SaveCurrentProfileName(profileName);
    }

    protected override void SaveProfileList()
    {
        PlayerPrefsRepository.SaveProfileList(_profileList);
    }
}
