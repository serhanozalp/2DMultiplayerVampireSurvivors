using System.Threading.Tasks;

namespace Interfaces
{
    public interface IConnectionCommand
    {
        public Task<bool> Execute();

        public Task Undo();
    }
}

