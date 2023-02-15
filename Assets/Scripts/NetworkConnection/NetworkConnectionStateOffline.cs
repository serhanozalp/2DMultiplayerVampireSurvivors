using Abstracts;
using System;
using UnityEngine;

public class NetworkConnectionStateOffline : BaseNetworkConnectionState
{
    public NetworkConnectionStateOffline(NetworkConnectionStateMachine networkConnectionStateMachine) : base(networkConnectionStateMachine)
    {
    }

    public override void Enter()
    {
        if(!String.IsNullOrEmpty(_networkManager.DisconnectReason))
        {
            _connectionEventMessageChannel.Publish(JsonUtility.FromJson<ConnectionEventMessage>(_networkManager.DisconnectReason));
        }
        _networkManager.Shutdown();
    }

    public override void Exit()
    { 
    }
}
