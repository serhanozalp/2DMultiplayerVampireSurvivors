using System;
using Interfaces;

namespace Abstracts
{
    public abstract class BaseMessageChannel<T> : BaseRuntimeList<Action<T>>, IMessageChannel<T>
    {
        public abstract void Publish(T message);

        public abstract void Subscribe(Action<T> action);

        public abstract void Unsubscribe(Action<T> action);
    }
}



