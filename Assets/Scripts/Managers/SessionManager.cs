using Abstracts;

public class SessionManager : BaseSessionManager
{
    public override void AddPlayerSessionData(ulong clientNetworkId, SessionData sessionData)
    {
        _networkSessionDictionary.Add(clientNetworkId, sessionData);
    }

    public override void DeletePlayerSessionData(ulong clientNetworkId)
    {
        _networkSessionDictionary.Remove(clientNetworkId);
    }

    public override SessionData GetPlayerSessionData(ulong clientNetworkId)
    {
        return _networkSessionDictionary[clientNetworkId];
    }
}
