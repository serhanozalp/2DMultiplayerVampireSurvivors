using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Abstracts;

public class LobbyServiceFacade : BaseLobbyServiceFacade
{
    public override async void TrySendHeartBeatPingAsync(string lobbyId)
    {
        try
        {
            await LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while sending heartbeat to the lobby!");
            throw;
        }
    }

    public override async Task TryDeleteLobbyAsync(string lobbyId)
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(lobbyId);
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while deleting the lobby!");
            throw;
        }
    }

    public override async Task TryRemovePlayerAsync(string lobbyId, string playerId)
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while removing the player from lobby!");
            throw;
        }
    }

    public override async Task<Lobby> TryJoinLobbyByIdAsync(string lobbyId)
    {
        try
        {
            var lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            return lobby;
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while joining lobby!");
            throw;
        }
    }

    public override async Task<Lobby> TryJoinLobbyByCodeAsync(string lobbyCode)
    {
        try
        {
            var lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            return lobby;
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while joining lobby!");
            throw;
        }
    }

    public override async Task<Lobby> TryCreateLobbyAsync(string lobbyName, CreateLobbyOptions createLobbyOptions)
    {
        try
        {
            var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, ConstantDictionary.GAMEPLAY_MAX_PLAYERS, createLobbyOptions);
            return lobby;
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while creating lobby!");
            throw;
        }
    }

    public override async Task<List<Lobby>> TryQueryLobbiesAsync(QueryLobbiesOptions queryLobbiesOptions)
    {
        try
        {
            var queriedLobbies = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            return queriedLobbies.Results;
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while querrying lobbies!");
            throw;
        }
    }
}
