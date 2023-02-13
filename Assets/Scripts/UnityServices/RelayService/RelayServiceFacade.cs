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
    private readonly NetworkManager _networkManager;
    public RelayServiceFacade()
    {
        _networkManager = ServiceLocator.Instance.GetService<NetworkManager>(true);
    }
    public override async Task<(bool isSuccessful, string relayCode)> TryCreateAllocationAsync()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(ConstantDictionary.GAMEPLAY_MAX_PLAYERS);
            string relayCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            _networkManager.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, ConstantDictionary.RELAYSERVICE_CONNECTION_TYPE));
            return (true, relayCode);
        }
        catch (RelayServiceException)
        {
            // POPUP
            Debug.LogError("Error while allocating relay!");
            return (false, "");
        }
    }

    public override async Task<bool> TryJoinAllocationAsync(string relayCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayCode);
            _networkManager.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, ConstantDictionary.RELAYSERVICE_CONNECTION_TYPE));
            return true;
        }
        catch (RelayServiceException)
        {
            // POPUP
            Debug.LogError("Error while joining relay allocation!");
            return false;
        }
    }
}
