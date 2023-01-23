using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public static class ClientPrefs
{
    public static void SaveProfileList(List<string> profileList)
    {
        PlayerPrefs.SetString(KeyDictionary.KEY_CLIENTPREFS_PROFILE_LIST, profileList.ToStringSeperatedByComma());
    }
    public static List<string> LoadProfileList()
    {
        return PlayerPrefs.GetString(KeyDictionary.KEY_CLIENTPREFS_PROFILE_LIST,"").ToListSplitByComma();
    }

    public static void SaveCurrentProfileName(string profileName)
    {
        PlayerPrefs.SetString(KeyDictionary.KEY_CLIENTPREFS_PROFILE_NAME_CURRENT, profileName);
    }
    
    public static string LoadCurrentProfileName()
    {
        return PlayerPrefs.GetString(KeyDictionary.KEY_CLIENTPREFS_PROFILE_NAME_CURRENT, KeyDictionary.CLIENTPREFS_PROFILE_NAME_DEFAULT);
    }
}
