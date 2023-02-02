using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Unity.Services.Lobbies.Models;

namespace Abstracts
{
    public abstract class BaseLobbyServiceFacade 
    {
        public abstract Task<bool> TryCreateLobbyAsync(string lobbyName, Dictionary<Type, string> selectedGameModeNameDictionary);

        public abstract Task<List<Lobby>> TryQuerryLobbiesAsync(Dictionary<Type, string> selectedGameModeNameDictionary);
    }
}

