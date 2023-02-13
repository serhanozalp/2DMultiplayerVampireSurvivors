using Abstracts;
using Unity.Netcode;
using UnityEngine;

public class NetworkConnectionStateStartingHost : BaseNetworkConnectionStateConnecting
{
    private readonly SessionManager _sessionManager;

    public NetworkConnectionStateStartingHost(NetworkConnectionStateMachine networkConnectionStateMachine) : base(networkConnectionStateMachine)
    {
        _sessionManager = ServiceLocator.Instance.GetService<SessionManager>(true);
    }

    public override void Enter()
    {
        base.Enter();
        _networkManager.ConnectionApprovalCallback += NetworkManager_OnConnectionApprovalCallBack;
        StartHost();
    }

    public override void Exit()
    {
        base.Exit();
        _networkManager.ConnectionApprovalCallback -= NetworkManager_OnConnectionApprovalCallBack;
    }

    private void StartHost()
    {
        SetNetworkConnectionData();
        if (!_networkManager.StartHost())
        {
            // POPUP
            Debug.LogError("Error while starting host!");
            _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateOffline);
        }
    }

    protected override void NetworkManager_OnClientConnectedCallBack(ulong clientNetworkId)
    {
        _networkManager.ConnectedClients[clientNetworkId].PlayerObject.GetComponent<PlayerNetworkObject>().PlayerName = _sessionManager.GetPlayerSessionData(clientNetworkId).playerName;
        if (clientNetworkId == _networkManager.LocalClientId) _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateHosting);
    }

    private void NetworkManager_OnConnectionApprovalCallBack(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
        response.CreatePlayerObject = true;
        var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(System.Text.Encoding.UTF8.GetString(request.Payload));
        _sessionManager.AddPlayerSessionData(request.ClientNetworkId, new SessionData { clientNetworkId = request.ClientNetworkId, playerName = connectionPayload.playerName, unityServiceId = connectionPayload.unityServiceId });
    }
}
