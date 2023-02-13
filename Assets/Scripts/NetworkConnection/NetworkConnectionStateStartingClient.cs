using Abstracts;
using UnityEngine;

public class NetworkConnectionStateStartingClient : BaseNetworkConnectionStateConnecting
{
    public NetworkConnectionStateStartingClient(NetworkConnectionStateMachine networkConnectionStateMachine) : base(networkConnectionStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartClient();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void StartClient()
    {
        SetNetworkConnectionData();
        if (!_networkManager.StartClient())
        {
            // POPUP
            Debug.LogError("Error while starting client!");
            _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateOffline);
        }
    }

    protected override void NetworkManager_OnClientConnectedCallBack(ulong clientId)
    {
        _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateClientConnected);
    }
}
