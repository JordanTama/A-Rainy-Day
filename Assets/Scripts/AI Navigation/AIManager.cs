using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Principal;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIManager : ScriptableObject
{
    [Header("Simulation Settings")]
    [SerializeField] private float simulationSpeed;
    [SerializeField] private float defaultSimulationSpeed;

    [Header("Spatial Partitioning Settings")]
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int reach;

    private Grid _grid;
    private List<AIAgent> _agents = new List<AIAgent>();
    private List<AISpawner> _spawners = new List<AISpawner>();
    private float _speed;

    public float Speed => _speed;
    public AIAgent[] Navigators => _agents.ToArray();
    public AISpawner[] Spawners => _spawners.ToArray();


    public void Initialize(Matrix4x4 matrix)
    {
        _agents = new List<AIAgent>();
        _spawners = new List<AISpawner>();
        simulationSpeed = defaultSimulationSpeed;

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
        {
            _agents.Add(agent);
            _grid.Add(agent);
        }
    }

    public void RemoveAgent(AIAgent agent)
    {
        if (_agents.Contains(agent))
        {
            _agents.Remove(agent);
            _grid.Remove(agent);
        }
    }

    public void UpdateAgent(AIAgent agent, Vector3 previousPosition)
    {
        if (_agents.Contains(agent))
        {
            _grid.Move(agent, previousPosition);
        }
    }

    public AIAgent[] GetAgents(AIAgent agent)
    {
        return _grid.GetAgents(agent.transform.position, reach);
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


        public void Add(AIAgent agent)
        {
            int index = CalculateIndex(agent.transform.position);

            int cellX = index % _width;
            int cellY = index / _width;

            agent.prev = null;
            agent.next = _cells[cellX][cellY];
            _cells[cellX][cellY] = agent;

            if (agent.next != null)
            {
                agent.next.prev = agent;
            }
        }

        public void Remove(AIAgent agent)
        {
            int index = CalculateIndex(agent.transform.position);

            int cellX = index % _width;
            int cellY = index / _width;

            if (agent.prev != null)
            {
                agent.prev.next = agent.next;
            }

            if (agent.next != null)
            {
                agent.next.prev = agent.prev;
            }

            if (_cells[cellX][cellY] == agent)
            {
                _cells[cellX][cellY] = agent.next;
            }

            agent.next = null;
            agent.prev = null;
        }

        public void Move(AIAgent agent, Vector3 oldPosition)
        {
            int oldIndex = CalculateIndex(oldPosition);
            int index = CalculateIndex(agent.transform.position);

            if (index == oldIndex) return;

            int oldCellX = oldIndex % _width;
            int oldCellY = oldIndex / _width;

            if (agent.prev != null)
                agent.prev.next = agent.next;

            if (agent.next != null)
                agent.next.prev = agent.prev;

            if (_cells[oldCellX][oldCellY] == agent)
                _cells[oldCellX][oldCellY] = agent.next;

            agent.next = null;
            agent.prev = null;

            Add(agent);
        }

        public AIAgent[] GetAgents(Vector3 origin, int reach = 0)
        {
            List<AIAgent> agents = new List<AIAgent>();
            int index = CalculateIndex(origin);

            for (int wReach = -reach; wReach <= reach; wReach++)
            {
                for (int hReach = -reach; hReach <= reach; hReach++)
                {
                    int w = GetWidthIndex(index) + wReach;
                    int h = GetHeightIndex(index) + hReach;

                    if (w >= 0 && w < _width && h >= 0 && h < _height)
                    {
                        agents.Add(_cells[w][h]);
                    }
                }
            }

            return agents.ToArray();
        }

        public int CalculateIndex(Vector3 worldPosition)
        {
            Vector3 localPosition = _matrix.inverse.MultiplyPoint(worldPosition);
            localPosition += new Vector3(0.5f, 0, 0.5f);

            int cellX = (int) (localPosition.x / (1f / _width));
            int cellY = (int) (localPosition.z / (1f / _height));

            int index = (cellY * _width) + cellX;

            return index;
        }

        private int GetWidthIndex(int index) => index % _width;

        private int GetHeightIndex(int index) => index / _width;

        private int GetIndex(int widthIndex, int heightIndex) => (heightIndex * _width) + widthIndex;

        public void DrawGrid()
        {
            Handles.matrix = _matrix;
            Handles.color = Color.black;

            for (int itWidth = 0; itWidth < _width; itWidth++)
            {
                for (int itHeight = 0; itHeight < _height; itHeight++)
                {
                    DrawCell(itWidth, itHeight);
                }
            }
        }

        private void DrawCell(int cellX, int cellY)
        {
            Vector3 cellSize = new Vector3(1f / _width, 0, 1f / _height);

            Vector3 cellCorner = new Vector3(-.5f, 0, -.5f);

            cellCorner.x += cellX * (1f / _width);
            cellCorner.z += cellY * (1f / _height);
            cellCorner += cellSize * .5f;

            Handles.DrawWireCube(cellCorner, cellSize);

            string label = cellX + " : " + cellY + " (" + (cellY * _width + cellX) + ")";

            int limit = 500;
            int numAgents = 0;
            AIAgent current = _cells[cellX][cellY];
            bool capped = false;
            for (int i = 0; i < limit && current != null; i++)
            {
                if (i == limit - 1) capped = true;

                numAgents++;
                current = current.next;
            }

            label += "\n" + (capped ? limit + "+" : numAgents + "") + " Agents";

            Handles.Label(cellCorner, label);
        }
    }
}
