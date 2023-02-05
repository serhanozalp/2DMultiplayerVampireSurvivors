using System.Threading.Tasks;
using Unity.Services.Relay;

namespace Abstracts
{
    public abstract class BaseRelayServiceFacade 
    {
        public abstract Task<bool> TryCreateAllocationAsync();
    }
}

