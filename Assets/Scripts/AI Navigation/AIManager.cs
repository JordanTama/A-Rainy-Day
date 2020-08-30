using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIManager : ScriptableObject
{
    [Header("System Settings")] 
    [SerializeField] private float simulationSpeed;
    
    private List<AIAgent> _navigators = new List<AIAgent>();
    private List<AISpawner> _spawners = new List<AISpawner>();


    public float Speed => simulationSpeed;
    public AIAgent[] Navigators => _navigators.ToArray();
    public AISpawner[] Spawners => _spawners.ToArray();


    public void Initialize()
    {
        _navigators = new List<AIAgent>();
        _spawners = new List<AISpawner>();
    }
    
    public void AddNavigator(AIAgent agent)
    {
        if (!_navigators.Contains(agent))
            _navigators.Add(agent);
    }

    public void RemoveNavigator(AIAgent agent)
    {
        if (_navigators.Contains(agent))
            _navigators.Remove(agent);
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
