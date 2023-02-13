using System.Collections.Generic;

namespace Abstracts
{
    public struct SessionData
    {
        public ulong clientNetworkId;
        public string unityServiceId;
        public string playerName;

    }

    public abstract class BaseSessionManager
    {
        protected Dictionary<ulong, SessionData> _networkSessionDictionary = new Dictionary<ulong, SessionData>();

        public abstract void AddPlayerSessionData(ulong clientNetworkId, SessionData sessionData);

        public abstract void DeletePlayerSessionData(ulong clientNetworkId);

        public abstract SessionData GetPlayerSessionData(ulong clientNetworkId);
    }
}

