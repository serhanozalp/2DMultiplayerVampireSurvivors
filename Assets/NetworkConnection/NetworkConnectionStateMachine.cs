using Abstracts;
using UnityEngine;

public struct ConnectionPayload
{
    public string unityServiceId;
    public string playerName;
}

public class NetworkConnectionStateMachine
{
    public readonly BaseNetworkConnectionState _networkConnectionStateOffline;
    public readonly BaseNetworkConnectionState _networkConnectionStateStartingHost;
    public readonly BaseNetworkConnectionState _networkConnectionStateHosting;
    public readonly BaseNetworkConnectionState _networkConnectionStateStartingClient;
    public readonly BaseNetworkConnectionState _networkConnectionStateClientConnected;

    private BaseNetworkConnectionState _currentNetworkConnectionState;

    public NetworkConnectionStateMachine()
    {
        _networkConnectionStateOffline = new NetworkConnectionStateOffline(this);
        _networkConnectionStateStartingHost = new NetworkConnectionStateStartingHost(this);
        _networkConnectionStateHosting = new NetworkConnectionStateHosting(this);
        _networkConnectionStateStartingClient = new NetworkConnectionStateStartingClient(this);
        _networkConnectionStateClientConnected = new NetworkConnectionStateClientConnected(this);

        ChangeState(_networkConnectionStateOffline);
    }

    public void ChangeState(BaseNetworkConnectionState networkConnectionState)
    {
        if (networkConnectionState == _currentNetworkConnectionState) return;
        Debug.Log($"Changing To { networkConnectionState.GetType().Name } from { _currentNetworkConnectionState?.GetType().Name }");
        _currentNetworkConnectionState?.Exit();
        _currentNetworkConnectionState = networkConnectionState;
        networkConnectionState.Enter();
    }

    public void RequestShutdown()
    {
        _currentNetworkConnectionState.OnRequestShutdown();
    }
}
