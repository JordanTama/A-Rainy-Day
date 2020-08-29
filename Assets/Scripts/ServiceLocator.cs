using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface IGameService { }

public class ServiceLocator
{
    private ServiceLocator()
    {
        // Put any manager scripts you might need in here
        // Register them with
        // Register(PlayerManager);
        Register(new InputManager());
        Register(new GameLoopManager());
        Register(new CameraManager(Get<InputManager>()));
        Register(new TileManager(Get<CameraManager>(), Get<InputManager>(),Get<GameLoopManager>()));
        
    }

    public static ServiceLocator Current { get; private set; }

    private readonly Dictionary<string, IGameService> services = new Dictionary<string, IGameService>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        Current = new ServiceLocator();
        Debug.Log("Service Locator Initialized");
    }

    public void Register<T>(T service) where T : IGameService
    {
        string key = typeof(T).Name;
        if (services.ContainsKey(key))
        {
            Debug.Log($"{key} has already been registered.");
            throw new InvalidOperationException();
        }

        services.Add(key, service);
        Debug.Log($"{key} has been registered.");
    }

    public void Unregister<T>(T service) where T : IGameService
    {
        string key = typeof(T).Name;
        if (!services.ContainsKey(key))
        {
            Debug.Log($"{key} does not exist.");
            throw new InvalidOperationException();
        }

        services.Remove(key);
        Debug.Log($"{key} has been unregistered.");
    }

    public T Get<T>() where T : IGameService
    {
        string key = typeof(T).Name;
        if (!services.ContainsKey(key))
        {
            Debug.Log($"{key} does not exist.");
            throw new InvalidOperationException();
        }

        return (T)services[key];
    }
}
