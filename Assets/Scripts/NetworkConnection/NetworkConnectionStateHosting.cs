using Abstracts;
using Unity.Netcode;
using UnityEngine;

public class NetworkConnectionStateHosting : BaseNetworkConnectionStateOnline
{
    private readonly SessionManager _sessionManager;

    public NetworkConnectionStateHosting(NetworkConnectionStateMachine networkConnectionStateMachine) : base(networkConnectionStateMachine)
    {
        _sessionManager = ServiceLocator.Instance.GetService<SessionManager>(true);
    }

    public override void Enter()
    {
        base.Enter();
        _networkManager.ConnectionApprovalCallback += NetworkManager_OnConnectionApprovalCallBack;
        _networkManager.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallBack;
    }

    public override void Exit()
    {
        base.Exit();
        _networkManager.ConnectionApprovalCallback -= NetworkManager_OnConnectionApprovalCallBack;
        _networkManager.OnClientConnectedCallback -= NetworkManager_OnClientConnectedCallBack;
    }

    private void NetworkManager_OnClientConnectedCallBack(ulong clientNetworkId)
    {
        _networkManager.ConnectedClients[clientNetworkId].PlayerObject.GetComponent<PlayerNetworkObject>().PlayerName = _sessionManager.GetPlayerSessionData(clientNetworkId).playerName;
    }

    private void NetworkManager_OnConnectionApprovalCallBack(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
        response.CreatePlayerObject = true;
        var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(System.Text.Encoding.UTF8.GetString(request.Payload));
        _sessionManager.AddPlayerSessionData(request.ClientNetworkId, new SessionData { clientNetworkId = request.ClientNetworkId, playerName = connectionPayload.playerName, unityServiceId = connectionPayload.unityServiceId });
    }

    protected override void OnClientDisconnect(ulong clientNetworkId)
    {
        _sessionManager.DeletePlayerSessionData(clientNetworkId);
    }

    public override void OnRequestShutdown()
    {
        _sessionManager.DeletePlayerSessionData(_networkManager.LocalClientId);
        base.OnRequestShutdown();
    }
}
