using System;

namespace Interfaces
{
    public interface IMessageChannel<T>
    {
        public void Publish(T message);

        public void Subscribe(Action<T> action);

        public void Unsubscribe(Action<T> action);
    }
}

