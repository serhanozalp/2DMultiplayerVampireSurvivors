using System;
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
        }
    }

    public override async Task<bool> TryDeleteLobbyAsync(string lobbyId)
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(lobbyId);
            return true;
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while deleting the lobby!");
            return false;
        }
    }

    public override async Task<bool> TryRemovePlayerAsync(string lobbyId, string playerId)
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);
            return true;
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while removing the player from lobby!");
            return false;
        }
    }

    public override async Task<(bool isSuccessful, Lobby joinedLobby)> TryJoinLobbyByIdAsync(string lobbyId)
    {
        try
        {
            var lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            return (true, lobby);
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while joining lobby!");
            return (false, null);
        }
    }

    public override async Task<(bool isSuccessful, Lobby joinedLobby)> TryJoinLobbyByCodeAsync(string lobbyCode)
    {
        try
        {
            var lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            return (true, lobby);
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while joining lobby!");
            return (false, null);
        }
    }

    public override async Task<(bool isSuccessful, Lobby createdLobby)> TryCreateLobbyAsync(string lobbyName, CreateLobbyOptions createLobbyOptions)
    {
        try
        {
            if (!IsLobbyNameValid(lobbyName))
            {
                PopupManager.Instance.AddPopup("Lobby Name Error", "Lobby Name Is Not Valid!");
                return (false, null);
            }
            var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, ConstantDictionary.GAMEPLAY_MAX_PLAYERS, createLobbyOptions);
            return (true, lobby);
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while creating lobby!");
            return (false, null);
        }
    }

    public override async Task<(bool isSuccessful, List<Lobby> queriedLobbyList)> TryQueryLobbiesAsync(QueryLobbiesOptions queryLobbiesOptions)
    {
        try
        {
            var queriedLobbies = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            return (true, queriedLobbies.Results);
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while querrying lobbies!");
            return (false, null);
        }
    }
}
