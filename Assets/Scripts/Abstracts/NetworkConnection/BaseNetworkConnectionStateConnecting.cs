using Unity.Services.Authentication;
using UnityEngine;

namespace Abstracts
{
    public abstract class BaseNetworkConnectionStateConnecting : BaseNetworkConnectionState
    {
        public BaseNetworkConnectionStateConnecting(NetworkConnectionStateMachine networkConnectionStateMachine) : base(networkConnectionStateMachine)
        {
        }

        public override void Enter()
        {
            _networkManager.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallBack;
        }

        public override void Exit()
        {
            _networkManager.OnClientConnectedCallback -= NetworkManager_OnClientConnectedCallBack;
        }

        protected abstract void NetworkManager_OnClientConnectedCallBack(ulong clientNetworkId);
        

        protected void SetNetworkConnectionData()
        {
            var connectionPayloadJson = JsonUtility.ToJson(new ConnectionPayload { unityServiceId = AuthenticationService.Instance.PlayerId, playerName = AuthenticationService.Instance.Profile });
            var connectionPayloadByte = System.Text.Encoding.UTF8.GetBytes(connectionPayloadJson);
            _networkManager.NetworkConfig.ConnectionData = connectionPayloadByte;
        }
    }
}

