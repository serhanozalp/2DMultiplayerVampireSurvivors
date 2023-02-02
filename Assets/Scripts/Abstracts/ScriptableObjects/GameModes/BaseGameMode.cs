using System;
using UnityEngine;

namespace Abstracts
{
    public abstract class BaseGameMode : ScriptableObject, IComparable<BaseGameMode>
    {
        public abstract string ModeName { get; }

        public abstract int CompareTo(BaseGameMode other);
    }
}

