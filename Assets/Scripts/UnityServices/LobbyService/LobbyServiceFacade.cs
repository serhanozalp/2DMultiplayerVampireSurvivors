using Unity.Services.Lobbies;

public class LobbyServiceFacade
{
    private const int k_maxPlayers = 4; 
    public async void TryCreateLobbyAsync(string lobbyName)
    {
        try
        {
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions { IsPrivate = false };
            await LobbyService.Instance.CreateLobbyAsync(lobbyName, k_maxPlayers);
        }
        catch (LobbyServiceException)
        {

        }
        
    }
}
