using Abstracts;
using UnityEngine;

public class NetworkConnectionStateStartingClient : BaseNetworkConnectionStateOnline
{
    public NetworkConnectionStateStartingClient(NetworkConnectionStateMachine networkConnectionStateMachine) : base(networkConnectionStateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _networkManager.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        StartClient();
    }

    public override void Exit()
    {
        base.Exit();
        _networkManager.OnClientConnectedCallback -= NetworkManager_OnClientConnectedCallback;
    }

    private void StartClient()
    {
        if (!_networkManager.StartClient())
        {
            // POPUP
            Debug.LogError("Error while starting client!");
            _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateOffline);
        }
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateClientConnected);
    }
}
