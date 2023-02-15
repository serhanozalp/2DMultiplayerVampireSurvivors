using System.Collections.Generic;
using System.Threading.Tasks;
using Interfaces;
using UnityEngine;

public class ConnectionCommandQueue : IReset
{
    private readonly Queue<IConnectionCommand> _connectionCommandProcessQueue = new Queue<IConnectionCommand>();
    private readonly Stack<IConnectionCommand> _connectionCommandRollbackStack = new Stack<IConnectionCommand>();

    public void AddCommand(IConnectionCommand connectionCommand)
    {
        _connectionCommandProcessQueue.Enqueue(connectionCommand);
    }

    public async Task<bool> Process()
    {
        while (_connectionCommandProcessQueue.Count > 0)
        {
            var command = _connectionCommandProcessQueue.Dequeue();
            if (!await command.Execute())
            {
                await RollBack();
                return false;
            }
            _connectionCommandRollbackStack.Push(command);
        }
        return true;
    }

    public async Task RollBack()
    {
        Debug.LogError("Rolling Back!");
        while (_connectionCommandRollbackStack.Count > 0)
        {
            await _connectionCommandRollbackStack.Pop().Undo();
        }
    }

    public void Reset()
    {
        _connectionCommandProcessQueue.Clear();
        _connectionCommandRollbackStack.Clear();
    }
}
