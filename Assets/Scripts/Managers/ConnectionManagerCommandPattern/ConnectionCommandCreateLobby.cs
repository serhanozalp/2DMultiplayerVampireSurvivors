using UnityEngine;
using Interfaces;
using System.Threading.Tasks;
using Abstracts;
using Unity.Services.Lobbies;
using System.Collections.Generic;
using System;
using Unity.Services.Lobbies.Models;

public class ConnectionCommandCreateLobby : IConnectionCommand
{
    private readonly BaseLobbyServiceFacade _lobbyServiceFacade;
    private readonly LocalLobby _localLobby;
    private readonly LobbyPing _lobbyPing;
    private readonly string _lobbyName;
    private readonly Dictionary<Type, string> _selectedGameModeNameDictionary;

    public ConnectionCommandCreateLobby(BaseLobbyServiceFacade lobbyServiceFacade, LocalLobby localLobby, LobbyPing lobbyPing, string lobbyName, Dictionary<Type,string> selectedGameModeNameDictionary)
    {
        _lobbyServiceFacade = lobbyServiceFacade;
        _localLobby = localLobby;
        _lobbyPing = lobbyPing;
        _lobbyName = lobbyName;
        _selectedGameModeNameDictionary = selectedGameModeNameDictionary;
    }
    public async Task<bool> Execute()
    {
        Debug.LogWarning("Executing Create Lobby");
        var createLobbyRequestResult = await _lobbyServiceFacade.TryCreateLobbyAsync(_lobbyName, GenerateCreateLobbyOptions(_localLobby.RelayCode, _selectedGameModeNameDictionary));
        if (createLobbyRequestResult.isSuccessful)
        {
            var createdLobby = createLobbyRequestResult.createdLobby;
            _localLobby.SetLobbyData(createdLobby);
            _lobbyPing.StartPing(createdLobby.Id);
        }
        return createLobbyRequestResult.isSuccessful;
    }

    public async Task Undo()
    {
        Debug.LogWarning("Undoing Create Lobby");
        if (_localLobby.IsActive) 
        {
            if (await _lobbyServiceFacade.TryDeleteLobbyAsync(_localLobby.LobbyId)) 
            {
                _localLobby.Reset();
                _lobbyPing.StopPing();
            } 
        }
    }

    private CreateLobbyOptions GenerateCreateLobbyOptions(string relayCode, Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
        {
            IsPrivate = false,
            Data = new Dictionary<string, DataObject>()
        };
        foreach (var pair in selectedGameModeNameDictionary)
        {
            Type type = pair.Key;
            createLobbyOptions.Data.Add(pair.Key.ToString(), new DataObject(
                DataObject.VisibilityOptions.Public,
                pair.Value,
                GameModeDataSource.GetGameModeByTypeAndModeName(type, pair.Value).DataObjectIndexOptions));
        }
        createLobbyOptions.Data.Add(ConstantDictionary.KEY_LOBBY_OPTIONS_RELAYCODE, new DataObject(
            DataObject.VisibilityOptions.Public, relayCode ?? ""));
        return createLobbyOptions;
    }
}
