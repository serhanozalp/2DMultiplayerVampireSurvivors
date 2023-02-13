using System.Threading.Tasks;

namespace Abstracts
{
    public abstract class BaseRelayServiceFacade 
    {
        public abstract Task<(bool isSuccessful, string relayCode)> TryCreateAllocationAsync();

        public abstract Task<bool> TryJoinAllocationAsync(string relayCode);
    }
}

