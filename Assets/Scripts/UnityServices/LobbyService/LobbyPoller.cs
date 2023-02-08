using Abstracts;
using System.Threading;
using System.Threading.Tasks;

public class LobbyPoller 
{
    private readonly BaseLobbyServiceFacade _lobbyServiceFacade;
    private readonly LocalLobby _lobbyToTrack;
    private readonly CancellationTokenSource _trackCancellationTokenSource;

    public LobbyPoller(BaseLobbyServiceFacade lobbyServiceFacade, LocalLobby lobbyToTrack)
    {
        _lobbyServiceFacade = lobbyServiceFacade;
        _lobbyToTrack = lobbyToTrack;
        _trackCancellationTokenSource = new CancellationTokenSource();
    }

    public void StartPolling()
    {
        if (_lobbyToTrack.IsPlayerTheHost) StartLobbyHeartBeat();
        StartLobbyUpdate();
    }

    public void StopPolling()
    {
        _trackCancellationTokenSource.Cancel();
    }

    private void StartLobbyHeartBeat()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                if (_trackCancellationTokenSource.IsCancellationRequested)
                {
                    break;
                }
                _lobbyServiceFacade.TrySendHeartBeatPingAsync(_lobbyToTrack.LobbyId);
                await Task.Delay(ConstantDictionary.LOBBYSERVICE_RATE_LIMIT_HEARTBEAT);
            }
        }, _trackCancellationTokenSource.Token);
    }

    private void StartLobbyUpdate()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                if (_trackCancellationTokenSource.IsCancellationRequested)
                {
                    break;
                }
                _lobbyServiceFacade.TryGetLobbyAsync(_lobbyToTrack.LobbyId);
                await Task.Delay(ConstantDictionary.LOBBYSERVICE_RATE_LIMIT_GET);
            }
        }, _trackCancellationTokenSource.Token);
    }
}
