using Abstracts;
using System.Threading;
using System.Threading.Tasks;

public class LobbyPing 
{
    private readonly BaseLobbyServiceFacade _lobbyServiceFacade;
    private CancellationTokenSource _pingCancellationTokenSource;

    public LobbyPing(BaseLobbyServiceFacade lobbyServiceFacade)
    {
        _lobbyServiceFacade = lobbyServiceFacade;
    }

    public void StartPing(string lobbyId)
    {
        _pingCancellationTokenSource = new CancellationTokenSource();
        Task.Run(async () =>
        {
            while (true)
            {
                if (_pingCancellationTokenSource.IsCancellationRequested)
                {
                    break;
                }
                _lobbyServiceFacade.TrySendHeartBeatPingAsync(lobbyId);
                await Task.Delay(ConstantDictionary.LOBBYSERVICE_RATE_LIMIT_HEARTBEAT, _pingCancellationTokenSource.Token);
            }
        }, _pingCancellationTokenSource.Token);
    }

    public void StopPing()
    {
        _pingCancellationTokenSource.Cancel();
    }
}
