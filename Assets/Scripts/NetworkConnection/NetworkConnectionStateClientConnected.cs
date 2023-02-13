using Abstracts;

public class NetworkConnectionStateClientConnected : BaseNetworkConnectionStateOnline
{
    public NetworkConnectionStateClientConnected(NetworkConnectionStateMachine networkConnectionStateMachine) : base(networkConnectionStateMachine)
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
}
