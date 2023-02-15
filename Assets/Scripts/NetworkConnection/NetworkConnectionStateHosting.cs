using Abstracts;
using Unity.Netcode;
using UnityEngine;

public class NetworkConnectionStateHosting : BaseNetworkConnectionStateOnline
{
    private readonly BaseSessionManager _sessionManager;

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
        if(_sessionManager.TryGetValue(clientNetworkId, out SessionData sessionData)) 
            _networkManager.ConnectedClients[clientNetworkId].PlayerObject.GetComponent<PlayerNetworkObject>().PlayerName = sessionData.playerName;
    }

    private void NetworkManager_OnConnectionApprovalCallBack(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (IsServerFull())
        {
            response.Reason = JsonUtility.ToJson(ConnectionEventMessage.DisconnectedServerFull);
            response.Approved = false;
            return;
        }
        response.Approved = true;
        response.CreatePlayerObject = true;
        var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(System.Text.Encoding.UTF8.GetString(request.Payload));
        _sessionManager.AddToDictionary(request.ClientNetworkId, new SessionData { clientNetworkId = request.ClientNetworkId, playerName = connectionPayload.playerName, unityServiceId = connectionPayload.unityServiceId });
    }

    protected override void OnClientDisconnect(ulong clientNetworkId)
    {
        if(clientNetworkId == _networkManager.LocalClientId)
        {
            _sessionManager.Reset();
            _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateOffline);
        }
        else _sessionManager.DeleteFromDictionary(clientNetworkId);
    }

    public override void OnRequestShutdown()
    {
        _sessionManager.Reset();
        DisconnectAllClients();
        base.OnRequestShutdown();
    }

    private void DisconnectAllClients()
    {
        for (int i = 0; i < _networkManager.ConnectedClientsIds.Count; i++)
        {
            var clientNetworkId = _networkManager.ConnectedClientsIds[i];
            if (clientNetworkId == _networkManager.LocalClientId) continue;
            _networkManager.DisconnectClient(clientNetworkId, JsonUtility.ToJson(ConnectionEventMessage.DisconnectedHostShutdown));
        }
    }

    private bool IsServerFull()
    {
        return _networkManager.ConnectedClientsList.Count >= ConstantDictionary.GAMEPLAY_MAX_PLAYERS;
    }
}
