using UnityEngine;
using Interfaces;
using System.Threading.Tasks;
using Abstracts;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;

public class ConnectionCommandQuitLobby : IConnectionCommand
{
    private readonly BaseLobbyServiceFacade _lobbyServiceFacade;
    private readonly LocalLobby _localLobby;
    private readonly LobbyPing _lobbyPing;

    public ConnectionCommandQuitLobby(BaseLobbyServiceFacade lobbyServiceFacade, LocalLobby localLobby, LobbyPing lobbyPing)
    {
        _lobbyServiceFacade = lobbyServiceFacade;
        _localLobby = localLobby;
        _lobbyPing = lobbyPing;
    }
    public async Task<bool> Execute()
    {
        try
        {
            Debug.LogWarning("Executing Quit Lobby");
            if (!_localLobby.IsActive) return true;
            if (_localLobby.IsPlayerTheHost)
            {
                await _lobbyServiceFacade.TryDeleteLobbyAsync(_localLobby.LobbyId);
            }
            else
            {
                await _lobbyServiceFacade.TryRemovePlayerAsync(_localLobby.LobbyId, AuthenticationService.Instance.PlayerId);
            }
            _lobbyPing.StopPing();
            _localLobby.Reset();
            return true;
        }
        catch (LobbyServiceException e)
        {
            if(e.Reason == LobbyExceptionReason.LobbyNotFound)
            {
                _lobbyPing.StopPing();
                _localLobby.Reset();
                return true;
            }
            return false;
        }
        
    }

    public async Task Undo()
    {
        Debug.LogWarning("Undoing Quit Lobby");
        await Task.CompletedTask;
    }
}
