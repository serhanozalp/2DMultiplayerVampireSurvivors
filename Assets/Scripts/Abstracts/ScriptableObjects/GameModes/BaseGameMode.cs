using System;
using UnityEngine;
using Unity.Services.Lobbies.Models;

namespace Abstracts
{
    public abstract class BaseGameMode : ScriptableObject, IComparable<BaseGameMode>
    {
        public abstract string ModeName { get; }
        public abstract string ModeTypeName { get; }
        public abstract DataObject.IndexOptions DataObjectIndexOptions { get; }

        public abstract int CompareTo(BaseGameMode other);
    }
}

