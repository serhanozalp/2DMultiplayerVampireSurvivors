using Abstracts;
using UnityEngine;

public class NetworkConnectionStateMachine
{
    public readonly BaseNetworkConnectionState _networkConnectionStateOffline;
    public readonly BaseNetworkConnectionState _networkConnectionStateStartingHost;
    public readonly BaseNetworkConnectionState _networkConnectionStateHosting;

    private BaseNetworkConnectionState _currentNetworkConnectionState;

    public NetworkConnectionStateMachine()
    {
        _networkConnectionStateOffline = new NetworkConnectionStateOffline(this);
        _networkConnectionStateStartingHost = new NetworkConnectionStateStartingHost(this);
        _networkConnectionStateHosting = new NetworkConnectionStateHosting(this);

        ChangeState(_networkConnectionStateOffline);
    }

    public void ChangeState(BaseNetworkConnectionState networkConnectionState)
    {
        Debug.Log($"Changing To { networkConnectionState.GetType().Name } from { _currentNetworkConnectionState?.GetType().Name }");
        _currentNetworkConnectionState?.Exit();
        _currentNetworkConnectionState = networkConnectionState;
        networkConnectionState.Enter();
    }
}
