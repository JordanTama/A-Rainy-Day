using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIManager : ScriptableObject
{
    [Header("System Settings")] 
    public float defaultSimulationSpeed;
    public float simulationSpeed;
    
    private List<AIAgent> _agents = new List<AIAgent>();
    private List<AISpawner> _spawners = new List<AISpawner>();

    [SerializeField] private float _speed;

    public float Speed => _speed;

    public AIAgent[] Navigators
    {
        get => _agents.ToArray();
        set => throw new System.NotImplementedException();
    }

    public AISpawner[] Spawners
    {
        get => _spawners.ToArray();
        set => throw new System.NotImplementedException();
    }


    public void Initialize()
    {
        _agents = new List<AIAgent>();
        _spawners = new List<AISpawner>();
        simulationSpeed = defaultSimulationSpeed;
        
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
        List<AISpawner> destinations = _spawners.FindAll(spawner => spawner.IsDestination && spawner != exclude);
        return destinations.Count == 0 ? exclude : destinations[Random.Range(0, destinations.Count)];
    }
}
