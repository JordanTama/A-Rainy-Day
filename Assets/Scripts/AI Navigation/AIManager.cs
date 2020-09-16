using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIManager : ScriptableObject
{
    [Header("Simulation Settings")] 
    [SerializeField] private float simulationSpeed;

    [Header("Spatial Partitioning Settings")]
    [SerializeField] private int width;
    [SerializeField] private int height;


    private Grid _grid;
    private List<AIAgent> _agents = new List<AIAgent>();
    private List<AISpawner> _spawners = new List<AISpawner>();
    private float _speed;

    
    public float Speed => _speed;
    public IEnumerable<AIAgent> Navigators => _agents.ToArray();
    public AISpawner[] Spawners => _spawners.ToArray();

    
    public void Initialize(Matrix4x4 matrix)
    {
        _agents = new List<AIAgent>();
        _spawners = new List<AISpawner>();
        
        _grid = new Grid(width, height, matrix);
        
        Pause();
    }

    public void DrawDebug()
    {
        _grid?.DrawGrid();
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
    
    
    public void AddAgent(AIAgent agent)
    {
        if (!_agents.Contains(agent))
            _agents.Add(agent);
    }

    public void RemoveAgent(AIAgent agent)
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

    private class Grid
    {
        private readonly int _width;
        private readonly int _height;

        private readonly Matrix4x4 _matrix;
        
        private AIAgent[][] _cells;


        public Grid(int width, int height, Matrix4x4 matrix)
        {
            _width = width;
            _height = height;

            _matrix = matrix;

            _cells = new AIAgent[width][];
            for (int i = 0; i < width; i++)
                _cells[i] = new AIAgent[height];
        }

        public int CalculateIndex(Vector3 worldPosition)
        {
            return 0;
        }
        
        public void DrawGrid()
        {
            Handles.matrix = _matrix;
            Vector3 corner = new Vector3(-.5f, 0, -.5f);

            Vector3 cellSize = new Vector3(1f / _width, 0, 1f / _height);
            
            // Boundary quad
            Handles.color = Color.black;
            Handles.DrawWireCube(Vector3.zero, new Vector3(1, 0, 1));
            
            for (int itWidth = 0; itWidth < _width; itWidth++)
            {
                for (int itHeight = 0; itHeight < _height; itHeight++)
                {
                    Vector3 cellCorner = corner;
                    cellCorner.x += itWidth * (1f / _width);
                    cellCorner.z += itHeight * (1f / _height);
                    cellCorner += cellSize * .5f;
                    
                    Handles.DrawWireCube(cellCorner, cellSize);
                    
                    Handles.Label(cellCorner, itWidth + " : " + itHeight + "(" + (itHeight * _width + itWidth) + ")");
                }
            }
        }
    }
}
