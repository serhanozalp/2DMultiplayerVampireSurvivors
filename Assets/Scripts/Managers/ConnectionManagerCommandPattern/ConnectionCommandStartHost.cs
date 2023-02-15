using UnityEngine;
using Interfaces;
using System.Threading.Tasks;
using Unity.Netcode;

public class ConnectionCommandStartHost : IConnectionCommand
{
    private readonly NetworkManager _networkManager;
    private readonly NetworkConnectionStateMachine _networkConnectionStateMachine;

    public ConnectionCommandStartHost(NetworkManager networkManager, NetworkConnectionStateMachine networkConnectionStateMachine)
    {
        _networkManager = networkManager;
        _networkConnectionStateMachine = networkConnectionStateMachine;
    }
    public async Task<bool> Execute()
    {
        Debug.LogWarning("Executing Start Host");
        _networkConnectionStateMachine.ChangeState(_networkConnectionStateMachine._networkConnectionStateStartingHost);
        return await AsyncTaskUtils.WaitUntil(() => { return _networkManager.IsListening; }, ConstantDictionary.NETWORK_CHECK_INTERVAL, ConstantDictionary.NETWORK_TIMEOUT_DURATION);
    }

    public async Task Undo()
    {
        Debug.LogWarning("Undoing Start Host");
        _networkConnectionStateMachine.RequestShutdown();
        await AsyncTaskUtils.WaitUntil(() => { return !_networkManager.IsListening; }, ConstantDictionary.NETWORK_CHECK_INTERVAL, ConstantDictionary.NETWORK_TIMEOUT_DURATION);
    }
}
