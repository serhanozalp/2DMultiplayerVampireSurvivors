using System.Collections.Generic;
using UnityEngine;
using Interfaces;

namespace Abstracts
{
    public abstract class BaseRuntimeDictionary<TKey,TValue> : IReset
    {
        protected readonly Dictionary<TKey, TValue> _runtimeDictionary = new Dictionary<TKey, TValue>();

        public virtual void AddToDictionary(TKey key, TValue value)
        {
            if (!DoesAlreadyExist(key)) _runtimeDictionary.Add(key, value);
            else Debug.LogWarning("Already exists in dictionary");
        }

        public virtual void DeleteFromDictionary(TKey key)
        {
            if (DoesAlreadyExist(key)) _runtimeDictionary.Remove(key);
            else Debug.LogWarning("Does not exists in dictionary");
        }

        public virtual bool TryGetValue(TKey key, out TValue value)
        {
            return _runtimeDictionary.TryGetValue(key, out value);
        }

        public virtual bool DoesAlreadyExist(TKey key)
        {
            return _runtimeDictionary.ContainsKey(key);
        }

        public virtual void Reset()
        {
            _runtimeDictionary.Clear();
        }
    }
}

