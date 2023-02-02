using System.Threading.Tasks;

namespace Abstracts
{
    public abstract class BaseAuthenticationServiceFacade
    {
        public abstract Task<bool> TryAuthorizePlayerAsync();
    }
}

