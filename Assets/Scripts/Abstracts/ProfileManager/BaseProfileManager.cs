using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Abstracts
{
    public abstract class BaseProfileManager
    {
        protected readonly List<string> _profileList;
        protected string _currentProfileName;

        public List<string> ProfileList { get => _profileList; }
        public string CurrentProfileName { get => _currentProfileName; set { SaveCurrentProfileName(value); _currentProfileName = value; } }

        protected abstract string RegexPattern { get; }
        
        public BaseProfileManager()
        {
            CurrentProfileName = LoadCurrentProfileName();
            _profileList = LoadProfileList();
        }
        public abstract void DeleteProfile(string profileName);

        public abstract void AddProfile(string profileName);

        protected abstract void SaveCurrentProfileName(string profileName);

        protected abstract string LoadCurrentProfileName();

        protected abstract void SaveProfileList();

        protected abstract List<string> LoadProfileList();

        protected bool IsProfileNameValid(string profileName)
        {
            return Regex.IsMatch(profileName, RegexPattern);
        }
    }
}