using System.Collections.Generic;
using Abstracts;
using Unity.Services.Lobbies.Models;
using System;

public class ConnectionManagerCommandPattern : BaseConnectionManager
{
    private readonly ConnectionCommandQueue _connectionCommandQueue = new ConnectionCommandQueue();

    public override void ShutdownAsync(bool forceQuit = false)
    {
        
    }

    public override async void StartClientAsync(Lobby lobby)
    {
        _connectionCommandQueue.Reset();
        _connectionCommandQueue.AddCommand(new ConnectionCommandAuthorizePlayer(_authenticationServiceFacade));
        _connectionCommandQueue.AddCommand(new ConnectionCommandJoinLobby(_lobbyServiceFacade, _localLobby, lobby));
        _connectionCommandQueue.AddCommand(new ConnectionCommandJoinAllocation(_relayServiceFacade, lobby.Data[ConstantDictionary.KEY_LOBBY_OPTIONS_RELAYCODE].Value));
        _connectionCommandQueue.AddCommand(new ConnectionCommandStartClient(_networkManager, _networkConnectionStateMachine));
        if (await _connectionCommandQueue.Process()) _connectionEventMessageChannel.Publish(ConnectionEventMessage.Connected);
        else _connectionEventMessageChannel.Publish(ConnectionEventMessage.StartingClientFailed);
    }

    public override async void StartHostAsync(string lobbyName, Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        _connectionCommandQueue.Reset();
        _connectionCommandQueue.AddCommand(new ConnectionCommandAuthorizePlayer(_authenticationServiceFacade));
        _connectionCommandQueue.AddCommand(new ConnectionCommandCreateAllocation(_relayServiceFacade, _localLobby));
        _connectionCommandQueue.AddCommand(new ConnectionCommandStartHost(_networkManager, _networkConnectionStateMachine));
        _connectionCommandQueue.AddCommand(new ConnectionCommandCreateLobby(_lobbyServiceFacade, _localLobby, _lobbyPing, lobbyName, selectedGameModeNameDictionary));
        if (await _connectionCommandQueue.Process()) _connectionEventMessageChannel.Publish(ConnectionEventMessage.Connected);
        else _connectionEventMessageChannel.Publish(ConnectionEventMessage.StartingHostFailed);
    }

    public override void QueryLobbies(Dictionary<Type, string> selectedGameModeNameDictionary)
    {
        _connectionCommandQueue.Reset();
        _connectionCommandQueue.AddCommand(new ConnectionCommandAuthorizePlayer(_authenticationServiceFacade));
        _connectionCommandQueue.AddCommand(new ConnectionCommandQuerryLobbies(_lobbyServiceFacade, selectedGameModeNameDictionary));
#pragma warning disable
        _connectionCommandQueue.Process();
#pragma warning enable
    }
}
