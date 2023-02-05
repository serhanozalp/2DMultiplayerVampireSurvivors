namespace Abstracts
{
    public abstract class BaseNetworkConnectionStateOnline : BaseNetworkConnectionState
    {
        public BaseNetworkConnectionStateOnline(NetworkConnectionStateMachine networkConnectionStateMachine) : base(networkConnectionStateMachine)
        {
        }

        public override void Enter()
        {
            _networkManager.OnClientDisconnectCallback += OnClientDisconnect;
        }

        public override void Exit()
        {
            _networkManager.OnClientDisconnectCallback -= OnClientDisconnect;
        }

        public abstract void OnClientDisconnect(ulong clientId);
    }
}

