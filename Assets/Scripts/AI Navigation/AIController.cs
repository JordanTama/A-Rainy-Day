using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [SerializeField] private AIManager manager;
    [SerializeField] private bool playOnAwake;
    private GameLoopManager _gameLoopManager;
    private TileManager _tileManager;
    private InteractableManager _interactableManager;
    
    [SerializeField] private NavMeshSurface navMeshSurface;

    private void Awake()
    {
        manager.Initialize();
        if (playOnAwake) manager.Play();
    }

    private void Start()
    {
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        _tileManager = ServiceLocator.Current.Get<TileManager>();
        _interactableManager = ServiceLocator.Current.Get<InteractableManager>();
        _gameLoopManager.OnPreparation += StopSpawning;
        _gameLoopManager.OnExecution += StartSpawning;
        _gameLoopManager.OnComplete += StopSpawning;

        _tileManager.OnNewTilePosition += BakeNavMesh;
        _tileManager.OnRebakeMesh += BakeNavMesh;
        _interactableManager.OnInteractableStateChange += BakeNavMesh;
    }

    private void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
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

    private void OnDestroy()
    {
        _gameLoopManager.OnPreparation -= StopSpawning;
        _gameLoopManager.OnExecution -= StartSpawning;
        _gameLoopManager.OnComplete -= StopSpawning;
        _tileManager.OnNewTilePosition -= BakeNavMesh;
        _tileManager.OnRebakeMesh -= BakeNavMesh;
    }
}
