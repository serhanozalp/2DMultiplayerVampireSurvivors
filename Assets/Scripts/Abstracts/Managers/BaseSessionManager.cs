namespace Abstracts
{
    public struct SessionData
    {
        public ulong clientNetworkId;
        public string unityServiceId;
        public string playerName;
    }

    public abstract class BaseSessionManager : BaseRuntimeDictionary<ulong, SessionData>
    {
    }
}

