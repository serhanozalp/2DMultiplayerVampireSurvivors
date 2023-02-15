using Interfaces;
using Abstracts;
using System.Threading.Tasks;
using UnityEngine;

public class ConnectionCommandCreateAllocation : IConnectionCommand
{
    private readonly BaseRelayServiceFacade _relayServiceFacade;
    private readonly LocalLobby _localLobby;

    public ConnectionCommandCreateAllocation(BaseRelayServiceFacade relayServiceFacade, LocalLobby localLobby)
    {
        _relayServiceFacade = relayServiceFacade;
        _localLobby = localLobby;
    }
    public async Task<bool> Execute()
    {
        Debug.LogWarning("Executing Create Allocation");
        var createAllocationRequestResult = await _relayServiceFacade.TryCreateAllocationAsync();
        _localLobby.RelayCode = createAllocationRequestResult.relayCode;
        return createAllocationRequestResult.isSuccessful;
    }

    public async Task Undo()
    {
        Debug.LogWarning("Undoing Create Allocation");
        await Task.CompletedTask;
    }
}
