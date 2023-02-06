using System.Threading.Tasks;

namespace Abstracts
{
    public abstract class BaseRelayServiceFacade 
    {
        public abstract Task<bool> TryCreateAllocationAsync();

        public abstract Task<bool> TryJoinAllocationAsync(string relayCode);
    }
}

