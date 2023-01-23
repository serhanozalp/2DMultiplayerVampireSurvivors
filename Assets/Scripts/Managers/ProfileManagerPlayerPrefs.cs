using System.Collections.Generic;
using UnityEngine;
using Abstracts;

public class ProfileManagerPlayerPrefs : BaseProfileManager
{
    protected override string RegexPattern => KeyDictionary.PROFILE_MANAGER_REGEX_PATTERN;

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
        if (profileName == _currentProfileName) CurrentProfileName = KeyDictionary.CLIENTPREFS_PROFILE_NAME_DEFAULT;
        SaveProfileList();
    }

    protected override string LoadCurrentProfileName()
    {
        return ClientPrefs.LoadCurrentProfileName();
    }

    protected override List<string> LoadProfileList()
    {
        return ClientPrefs.LoadProfileList();
    }

    protected override void SaveCurrentProfileName(string profileName)
    {
        ClientPrefs.SaveCurrentProfileName(profileName);
    }

    protected override void SaveProfileList()
    {
        ClientPrefs.SaveProfileList(_profileList);
    }
}
