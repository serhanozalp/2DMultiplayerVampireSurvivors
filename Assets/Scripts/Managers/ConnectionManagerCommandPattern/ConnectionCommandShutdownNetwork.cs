using UnityEngine;
using Interfaces;
using System.Threading.Tasks;
using Unity.Netcode;

public class ConnectionCommandShutdownNetwork : IConnectionCommand
{
    private readonly NetworkManager _networkManager;
    private readonly NetworkConnectionStateMachine _networkConnectionStateMachine;
    public ConnectionCommandShutdownNetwork(NetworkManager networkManager, NetworkConnectionStateMachine networkConnectionStateMachine)
    {
        _networkManager = networkManager;
        _networkConnectionStateMachine = networkConnectionStateMachine;
    }
    public async Task<bool> Execute()
    {
        Debug.LogWarning("Executing Shutdown Network");
        _networkConnectionStateMachine.RequestShutdown();
        return await AsyncTaskUtils.WaitUntil(() => { return !_networkManager.IsListening; }, ConstantDictionary.NETWORK_CHECK_INTERVAL, ConstantDictionary.NETWORK_TIMEOUT_DURATION);
    }

    public async Task Undo()
    {
        Debug.LogWarning("Undoing Shutdown Network");
        await Task.CompletedTask;
    }
}
