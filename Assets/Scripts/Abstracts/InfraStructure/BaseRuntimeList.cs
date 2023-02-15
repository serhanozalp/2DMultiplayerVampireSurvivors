using System.Collections.Generic;
using UnityEngine;
using Interfaces;

namespace Abstracts
{
    public abstract class BaseRuntimeList<T> : IReset
    {
        protected readonly List<T> _runtimeList = new List<T>();

        public virtual void AddToList(T item)
        {
            if (!DoesAlreadyExist(item)) _runtimeList.Add(item);
            else Debug.LogWarning("Already exists in list");
        }

        public virtual void DeleteFromList(T item)
        {
            if (DoesAlreadyExist(item)) _runtimeList.Remove(item);
            else Debug.LogWarning("Does not exists in list");
        }

        public virtual bool DoesAlreadyExist(T item)
        {
            return _runtimeList.Contains(item);
        }

        public virtual void Reset()
        {
            _runtimeList.Clear();
        }
    }
}

