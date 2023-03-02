using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using System.Threading.Tasks;
using Abstracts;
using Unity.Services.Lobbies;
using System;
using Unity.Services.Lobbies.Models;
using Extensions;

public class ConnectionCommandQuerryLobbies : IConnectionCommand
{
    private readonly BaseLobbyServiceFacade _lobbyServiceFacade;
    private readonly Dictionary<Type, string> _selectedGameModeNameDictionary;
    private readonly BaseMessageChannel<QueriedLobbyListMessage> _queriedLobbyListMessageChannel;
    public ConnectionCommandQuerryLobbies(BaseLobbyServiceFacade lobbyServiceFacade, Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        _lobbyServiceFacade = lobbyServiceFacade;
        _selectedGameModeNameDictionary = selectedGameModeNameDictionary;
        _queriedLobbyListMessageChannel = ServiceLocator.Instance.GetService<MessageChannel<QueriedLobbyListMessage>>();
    }
    public async Task<bool> Execute()
    {
        try
        {
            Debug.LogWarning("Executing Querry Lobbies");
            var queriedLobbies = await _lobbyServiceFacade.TryQueryLobbiesAsync(GenerateQueryLobbiesOptions(_selectedGameModeNameDictionary));
            _queriedLobbyListMessageChannel.Publish(new QueriedLobbyListMessage { queriedLobbyList = queriedLobbies });
            return true;
        }
        catch (LobbyServiceException)
        {
            return false;
        }
    }

    public async Task Undo()
    {
        Debug.LogWarning("Undoing Querry Lobbies");
        await Task.CompletedTask;
    }

    private QueryLobbiesOptions GenerateQueryLobbiesOptions(Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
        {
            Filters = new List<QueryFilter>()
        };
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
