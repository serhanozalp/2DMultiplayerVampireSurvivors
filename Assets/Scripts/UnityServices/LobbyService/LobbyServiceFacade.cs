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

    public LobbyServiceFacade()
    {
        _localLobby = ServiceLocator.Instance.GetService<LocalLobby>(true);
    }

    public override async Task<bool> TryJoinLobbyByIdAsync(string lobbyId)
    {
        try
        {
            var lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            _localLobby.ApplyLobbyData(lobby);
            return true;
        }
        catch (LobbyServiceException)
        {
            // POPUP
            Debug.LogError("Error while joining lobby!");
            return false;
        }
    }

    public override async Task<bool> TryCreateLobbyAsync(string lobbyName, Dictionary<Type,string> selectedGameModeNameDictionary)
    {
        try
        {
            if (!IsLobbyNameValid(lobbyName))
            {
                PopupManager.Instance.AddPopup("Lobby Name Error", "Lobby Name Is Not Valid!");
                return false;
            }
            var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, ConstantDictionary.GAMEPLAY_MAX_PLAYERS, GenerateCreateLobbyOptions(selectedGameModeNameDictionary));
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
        createLobbyOptions.Data.Add(ConstantDictionary.KEY_LOBBY_OPTIONS_RELAYCODE, new DataObject(
            DataObject.VisibilityOptions.Public, _localLobby.RelayCode ?? ""));
        return createLobbyOptions;
    }

    public override async Task<List<Lobby>> TryQueryLobbiesAsync(Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        try
        {
            var queriedLobbies = await LobbyService.Instance.QueryLobbiesAsync(GenerateQueryLobbiesOptions(selectedGameModeNameDictionary));
            return queriedLobbies.Results;
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
