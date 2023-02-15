using UnityEngine;
using Interfaces;
using System.Threading.Tasks;
using Unity.Netcode;

public class ConnectionCommandStartClient : IConnectionCommand
{
    private readonly NetworkManager _networkManager;
    private readonly NetworkConnectionStateMachine _networkConnectionStateMachine;

    public ConnectionCommandStartClient(NetworkManager networkManager, NetworkConnectionStateMachine networkConnectionStateMachine)
    {
        _networkManager = networkManager;
        _networkConnectionStateMachine = networkConnectionStateMachine;
    }
    public async Task<bool> Execute()
    {
        Debug.LogWarning("Executing Start Client");
        _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateStartingClient);
        return await AsyncTaskUtils.WaitUntil(() => { return _networkManager.IsConnectedClient; }, ConstantDictionary.NETWORK_CHECK_INTERVAL, ConstantDictionary.NETWORK_TIMEOUT_DURATION);
    }

    public async Task Undo()
    {
        Debug.LogWarning("Undoing Start Client");
        _networkConnectionStateMachine.RequestShutdown();
        await AsyncTaskUtils.WaitUntil(() => { return !_networkManager.IsConnectedClient; }, ConstantDictionary.NETWORK_CHECK_INTERVAL, ConstantDictionary.NETWORK_TIMEOUT_DURATION);
    }
}
