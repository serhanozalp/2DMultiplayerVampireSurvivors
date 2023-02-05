using Abstracts;

public class NetworkConnectionStateOffline : BaseNetworkConnectionState
{
    public NetworkConnectionStateOffline(NetworkConnectionStateMachine networkConnectionStateMachine) : base(networkConnectionStateMachine)
    {
    }

    public override void Enter()
    {
        _networkManager.Shutdown();
    }

    public override void Exit()
    { 
    }
}
