using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIManager : ScriptableObject
{
    [Header("System Settings")] 
    [SerializeField] private float simulationSpeed;
    
    private List<AIAgent> _agents = new List<AIAgent>();
    private List<AISpawner> _spawners = new List<AISpawner>();

    private float _speed;

    public float Speed => _speed;
    public AIAgent[] Navigators => _agents.ToArray();
    public AISpawner[] Spawners => _spawners.ToArray();


    public void Initialize()
    {
        _agents = new List<AIAgent>();
        _spawners = new List<AISpawner>();
        
        Pause();
    }

    [ContextMenu("Pause")]
    public void Pause()
    {
        _speed = 0f;
    }

    [ContextMenu("Play")]
    public void Play()
    {
        _speed = simulationSpeed;
    }
    
    [ContextMenu("Clear Agents")]
    public void ClearAgents()
    {
        for (int i = _agents.Count - 1; i >= 0; i--)
        {
            _agents[i].Clear();
        }
    }
    
    public void AddNavigator(AIAgent agent)
    {
        if (!_agents.Contains(agent))
            _agents.Add(agent);
    }

    public void RemoveNavigator(AIAgent agent)
    {
        if (_agents.Contains(agent))
            _agents.Remove(agent);
    }

    public void AddSpawner(AISpawner spawner)
    {
        if (!_spawners.Contains(spawner))
            _spawners.Add(spawner);
    }

    public void RemoveSpawner(AISpawner spawner)
    {
        if (_spawners.Contains(spawner))
            _spawners.Remove(spawner);
    }

    public AISpawner RandomSpawner(AISpawner exclude)
    {
        //AISpawner spawner = exclude;

        int index = Random.Range(0, _spawners.Count - 1);
        AISpawner spawner = index >= _spawners.IndexOf(exclude) ? _spawners[index + 1] : _spawners[index];
        
        // int range = Random.Range(0, _spawners.Count - 1);
        //
        // for (int i = 0; i <= range; i++)
        // {
        //     if (_spawners[i] == exclude) range++;
        //     spawner = _spawners[i];
        //     
        // }

        return spawner;
    }
}
