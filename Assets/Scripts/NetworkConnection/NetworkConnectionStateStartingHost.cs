using Abstracts;
using UnityEngine;

public class NetworkConnectionStateStartingHost : BaseNetworkConnectionStateOnline
{
    public NetworkConnectionStateStartingHost(NetworkConnectionStateMachine networkConnectionStateMachine) : base(networkConnectionStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _networkManager.OnServerStarted += NetworkManager_OnServerStarted;
        StartHost();
    }

    public override void Exit()
    {
        base.Exit();
        _networkManager.OnServerStarted -= NetworkManager_OnServerStarted;
    }

    private void StartHost()
    {
        if (!_networkManager.StartHost())
        {
            StartingHostFailed();
        }
    }

    private void StartingHostFailed()
    {
        // POPUP
        Debug.LogError("Error while starting host!");
        _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateOffline);
    }

    private void NetworkManager_OnServerStarted()
    {
        _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateHosting);
    }

    public override void OnClientDisconnect(ulong clientId)
    {
        if(clientId == _networkManager.LocalClientId) StartingHostFailed();
    }
}
