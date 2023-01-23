using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abstracts
{
    public abstract class MonoBehaviourPersistentSingleton<T> : MonoBehaviourSingleton<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this as T);
        }
    }
}
