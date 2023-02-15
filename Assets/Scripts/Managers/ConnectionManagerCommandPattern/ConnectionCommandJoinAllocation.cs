using UnityEngine;
using Interfaces;
using System.Threading.Tasks;
using Abstracts;

public class ConnectionCommandJoinAllocation : IConnectionCommand
{
    private readonly BaseRelayServiceFacade _relayServiceFacade;
    private readonly string _relayCode;

    public ConnectionCommandJoinAllocation(BaseRelayServiceFacade relayServiceFacade, string relayCode)
    {
        _relayServiceFacade = relayServiceFacade;
        _relayCode = relayCode;
    }
    public async Task<bool> Execute()
    {
        Debug.LogWarning("Executing Join Allocation");
        return await _relayServiceFacade.TryJoinAllocationAsync(_relayCode);
    }

    public async Task Undo()
    {
        Debug.LogWarning("Undoing Join Allocation");
        await Task.CompletedTask;
    }
}
