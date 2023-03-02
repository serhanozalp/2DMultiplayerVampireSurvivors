using UnityEngine;
using Abstracts;
using Interfaces;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using System;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;

public class ConnectionCommandJoinLobby : IConnectionCommand
{
    private readonly BaseLobbyServiceFacade _lobbyServiceFacade;
    private readonly LocalLobby _localLobby;
    private readonly Lobby _lobby;

    public ConnectionCommandJoinLobby(BaseLobbyServiceFacade lobbyServiceFacade, LocalLobby localLobby, Lobby lobby)
    {
        _lobbyServiceFacade = lobbyServiceFacade;
        _localLobby = localLobby;
        _lobby = lobby;
    }
    public async Task<bool> Execute()
    {
        try
        {
            Debug.LogWarning("Executing Join Lobby");
            var joinedLobby = !String.IsNullOrEmpty(_lobby.LobbyCode) ? await _lobbyServiceFacade.TryJoinLobbyByCodeAsync(_lobby.LobbyCode) : await _lobbyServiceFacade.TryJoinLobbyByIdAsync(_lobby.Id);
            _localLobby.SetLobbyData(joinedLobby);
            return true;
        }
        catch (LobbyServiceException)
        {
            return false;
        }
    }

    public async Task Undo()
    {
        try
        {
            Debug.LogWarning("Undoing Join Lobby");
            if (_localLobby.IsActive)
            {
                await _lobbyServiceFacade.TryRemovePlayerAsync(_localLobby.LobbyId, AuthenticationService.Instance.PlayerId);
                _localLobby.Reset();
            }
        }
        catch (LobbyServiceException)
        {
            await Task.CompletedTask;
        }
    }
}
