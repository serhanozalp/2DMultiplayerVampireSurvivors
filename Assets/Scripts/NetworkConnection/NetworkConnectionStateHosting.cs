using Abstracts;

public class NetworkConnectionStateHosting : BaseNetworkConnectionStateOnline
{
    public NetworkConnectionStateHosting(NetworkConnectionStateMachine networkConnectionStateMachine) : base(networkConnectionStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void OnClientDisconnect(ulong clientId)
    {
        if (clientId == _networkManager.LocalClientId) base.OnClientDisconnect(clientId);
    }
}
