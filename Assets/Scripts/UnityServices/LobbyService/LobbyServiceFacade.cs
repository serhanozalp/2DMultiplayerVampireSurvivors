using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Abstracts;
using Extensions;

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
            var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, k_maxPlayers, GenerateCreateLobbyOptions(selectedGameModeNameDictionary));
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

    private CreateLobbyOptions GenerateCreateLobbyOptions(Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions { IsPrivate = false };
        createLobbyOptions.Data = new Dictionary<string, DataObject>();
        foreach (var pair in selectedGameModeNameDictionary)
        {
            Type type = pair.Key;
            createLobbyOptions.Data.Add(pair.Key.ToString(), new DataObject(
                DataObject.VisibilityOptions.Public,
                pair.Value,
                GameModeDataSource.GetGameModeByTypeAndModeName(type, pair.Value).DataObjectIndexOptions));
        }
        return createLobbyOptions;
    }

    public override async Task<List<Lobby>> TryQuerryLobbiesAsync(Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        try
        {
            var queryResponse = await LobbyService.Instance.QueryLobbiesAsync(GenerateQueryLobbiesOptions(selectedGameModeNameDictionary));
            return queryResponse.Results;
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while querrying lobbies!");
            return null;
        }
    }

    private QueryLobbiesOptions GenerateQueryLobbiesOptions(Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions();
        queryLobbiesOptions.Filters = new List<QueryFilter>();
        foreach (var pair in selectedGameModeNameDictionary)
        {
            Type type = pair.Key;
            queryLobbiesOptions.Filters.Add(new QueryFilter(
                GameModeDataSource.GetGameModeByTypeAndModeName(type, pair.Value).DataObjectIndexOptions.ToQueryFilterFieldOptions(),
                pair.Value,
                QueryFilter.OpOptions.EQ));
        }
        return queryLobbiesOptions;
    }
}
