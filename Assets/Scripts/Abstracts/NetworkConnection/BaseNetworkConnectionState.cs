using Unity.Netcode;

namespace Abstracts
{
    public abstract class BaseNetworkConnectionState
    {
        protected readonly NetworkConnectionStateMachine _networkConnectionStateMachine;
        protected readonly NetworkManager _networkManager;

        public BaseNetworkConnectionState(NetworkConnectionStateMachine networkConnectionStateMachine)
        {
            _networkConnectionStateMachine = networkConnectionStateMachine;
            _networkManager = ServiceLocator.Instance.GetService<NetworkManager>();
        }
        public abstract void Enter();

        public abstract void Exit();

        public virtual void OnRequestShutdown() // This function can take ConnectStatus argument to specify why the user has requested shutdown
        {
            _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateOffline);
        }
    }
}

