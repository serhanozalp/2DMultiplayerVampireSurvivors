using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Abstracts;

public class LobbyServiceFacade : BaseLobbyServiceFacade
{
    private readonly LocalLobby _localLobby;
    private const int k_maxPlayers = 4;

    public LobbyServiceFacade()
    {
        _localLobby = ServiceLocator.Instance.GetService<LocalLobby>(true);
    }

    public override async Task<bool> TryCreateLobbyAsync(string lobbyName, Dictionary<Type,string> selectedGameModeNameDictionary)
    {
        try
        {
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions { IsPrivate = false };
            createLobbyOptions.Data = new Dictionary<string, DataObject>();
            foreach (var pair in selectedGameModeNameDictionary)
            {
                createLobbyOptions.Data.Add(pair.Key.ToString(), new DataObject(DataObject.VisibilityOptions.Public, pair.Value));
            }
            var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, k_maxPlayers, createLobbyOptions);
            _localLobby.ApplyLobbyData(lobby);
            return true;
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while creating lobby!");
            return false;
        }
    }

    public override async Task<List<Lobby>> TryQuerryLobbiesAsync(Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions(); // ADD FILTERS
            var queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            return queryResponse.Results;
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while querrying lobbies!");
            return null;
        }
    }
}
