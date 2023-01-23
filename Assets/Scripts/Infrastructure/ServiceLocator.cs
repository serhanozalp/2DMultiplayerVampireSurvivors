using System;
using System.Collections.Generic;
using UnityEngine;
using Abstracts;
using Extensions;

public class ServiceLocator : MonoBehaviourPersistentSingleton<ServiceLocator>
{
    [SerializeField]
    private MonoBehaviour[] _inSceneMonoBehavioursToRegister;

    private readonly Dictionary<Type, object> _servicesDictionary = new Dictionary<Type, object>();

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        RegisterInSceneMonoBehaviours();
    }

    private void RegisterInSceneMonoBehaviours()
    {
        foreach(var service in _inSceneMonoBehavioursToRegister)
        {
            dynamic castedService = Convert.ChangeType(service, service.GetType());
            RegisterService(castedService);
        }
    }

    public void RegisterService<TService>(TService service) where TService : class, new()
    {
        if (service == null) { Debug.LogWarning($"ServiceLocator: Cannot Register Type {typeof(TService)} because it is null!"); return; }
        if (IsRegistered<TService>()) { Debug.LogWarning($"ServiceLocator: Type {typeof(TService)} is already registered!"); return; }
        _servicesDictionary.Add(typeof(TService), service);
    }

    public TService GetService<TService>(bool forced = false) where TService : class, new()
    {
        if (IsRegistered<TService>()) return (TService)_servicesDictionary[typeof(TService)];
        if (!forced) { Debug.LogError($"ServiceLocator: {typeof(TService)} is not registered!"); return null; }
        var service = typeof(TService).IsMonoBehaviour() ? FindOrCreateMonoService<TService>() : new TService();
        _servicesDictionary.Add(typeof(TService), service);
        return service;
    }

    private TService FindOrCreateMonoService<TService>() where TService : class, new()
    {
        var serviceType = typeof(TService);
        var service = FindObjectOfType(serviceType);
        if(service == null)
        {
            var gameObject = new GameObject();
            gameObject.AddComponent(serviceType);
            gameObject.name = serviceType.Name;
            service = gameObject.GetComponent(serviceType);
        }
        return service as TService;
    }

    private bool IsRegistered<TService>()
    {
        return _servicesDictionary.ContainsKey(typeof(TService));
    }

    private void OnDestroy()
    {
        _servicesDictionary.Clear();
    }
}


