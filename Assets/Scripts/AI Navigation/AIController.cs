using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private AIManager manager;
    [SerializeField] private bool playOnAwake;
    private GameLoopManager _gameLoopManager;

    private void Awake()
    {
        manager.Initialize();
        if (playOnAwake) manager.Play();
    }

    private void Start()
    {
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        _gameLoopManager.OnPreparation += StopSpawning;
        _gameLoopManager.OnExecution += StartSpawning;
        _gameLoopManager.OnComplete += StopSpawning;
    }

    public void StartSpawning()
    {
        manager.Play();
    }

    public void StopSpawning()
    {
        manager.Pause();
        manager.ClearAgents();
    }
}
