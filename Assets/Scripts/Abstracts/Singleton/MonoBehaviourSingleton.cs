using UnityEngine;

namespace Abstracts
{
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }
        protected virtual void Awake()
        {
            if (Instance == null) Instance = this as T;
            else 
            { 
                Debug.LogWarning($"Destroying {typeof(T)}. It already exists in the scene.");
                Destroy(gameObject);
            }
        }
        private void Reset()
        {
            if (FindObjectsOfType<T>().Length > 1)
            {
                Debug.LogWarning($"Destroying {typeof(T)}. It already exists in the scene.");
                DestroyImmediate(this as T);
            }
        }
    }
}

