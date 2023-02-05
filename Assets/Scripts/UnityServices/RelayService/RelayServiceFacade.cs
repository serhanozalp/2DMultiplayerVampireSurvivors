using UnityEngine;
using Abstracts;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class RelayServiceFacade : BaseRelayServiceFacade
{
    private readonly LocalLobby _localLobby;
    private readonly NetworkManager _networkManager;
    public RelayServiceFacade()
    {
        _localLobby = ServiceLocator.Instance.GetService<LocalLobby>(true);
        _networkManager = ServiceLocator.Instance.GetService<NetworkManager>(true);
    }
    public override async Task<bool> TryCreateAllocationAsync()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(ConstantDictionary.GAMEPLAY_MAX_PLAYERS);
            string relayCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            _localLobby.RelayCode = relayCode;
            _networkManager.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, ConstantDictionary.RELAYSERVICE_CONNECTION_TYPE));
            return true;
        }
        catch (RelayServiceException)
        {
            // POPUP
            Debug.LogError("Error while allocating relay!");
            return false;
        }
    }
}
