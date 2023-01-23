using System;
using UnityEngine;

namespace Extensions
{
    public static class TypeExtensions 
    {
        public static bool IsMonoBehaviour(this Type type)
        {
            return typeof(MonoBehaviour).IsAssignableFrom(type);
        }
    }
}

