using Interfaces;
using Abstracts;
using System.Threading.Tasks;
using UnityEngine;

public class ConnectionCommandAuthorizePlayer : IConnectionCommand
{
    private readonly BaseAuthenticationServiceFacade _authenticationServiceFacade;
    public ConnectionCommandAuthorizePlayer(BaseAuthenticationServiceFacade authenticationServiceFacade)
    {
        _authenticationServiceFacade = authenticationServiceFacade;
    }
    public async Task<bool> Execute()
    {
        Debug.LogWarning("Executing Authorize Player");
        return await _authenticationServiceFacade.TryAuthorizePlayerAsync();
    }

    public async Task Undo()
    {
        Debug.LogWarning("Undoing Authorize Player");
        await Task.CompletedTask;
    }
}
