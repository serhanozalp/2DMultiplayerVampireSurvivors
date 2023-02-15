using System;
using Abstracts;

public class MessageChannel<T> : BaseMessageChannel<T>
{
    public override void Publish(T message)
    {
        foreach (var action in _runtimeList)
        {
            action?.Invoke(message);
        }
    }

    public override void Subscribe(Action<T> action)
    {
        _runtimeList.Add(action);
    }

    public override void Unsubscribe(Action<T> action)
    {
        _runtimeList.Remove(action);
    }
}
