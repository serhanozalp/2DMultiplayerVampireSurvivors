using UnityEngine;
using Abstracts;
using Interfaces;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using System;
using Unity.Services.Authentication;

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
        Debug.LogWarning("Executing Join Lobby");
        var joinLobbyRequestResult = !String.IsNullOrEmpty(_lobby.LobbyCode) ? await _lobbyServiceFacade.TryJoinLobbyByCodeAsync(_lobby.LobbyCode) : await _lobbyServiceFacade.TryJoinLobbyByIdAsync(_lobby.Id);
        if (joinLobbyRequestResult.isSuccessful) _localLobby.SetLobbyData(joinLobbyRequestResult.joinedLobby);
        return joinLobbyRequestResult.isSuccessful;
    }

    public async Task Undo()
    {
        Debug.LogWarning("Undoing Join Lobby");
        if (_localLobby.IsActive) 
        {
            if (await _lobbyServiceFacade.TryRemovePlayerAsync(_localLobby.LobbyId, AuthenticationService.Instance.PlayerId)) _localLobby.Reset();
        } 
    }
}
