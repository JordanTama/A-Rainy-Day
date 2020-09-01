using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class AISpawner : MonoBehaviour
{
    [Header("Component References")] 
    [SerializeField] private AIManager manager;
    
    [Header("Spawner Settings")]
    [SerializeField] private bool isDestination;
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private int maxSpawned;
    [SerializeField] private float spawnTick;

    [Header("Debug Settings")]
    [SerializeField] private bool liveUpdate;
    [SerializeField] private bool debugVisualization;
    [SerializeField] private bool debugLabels;
    [SerializeField] private Color visColor;
    [SerializeField] private float visHeight;
    [SerializeField] private float visOffset;
    
    private Transform[] _points;
    private int _spawned;
    private float _lastSpawnTime;

    private float SpawnTick => spawnTick / manager.Speed;
    private Vector3 Centre => Lerp(0.5f);
    private bool CanSpawn => Time.timeSinceLevelLoad - _lastSpawnTime > SpawnTick && _spawned < maxSpawned;
    public bool IsDestination => isDestination;

    private void Start()
    {
        manager.AddSpawner(this);
        
        UpdatePoints();

        _spawned = 0;
        _lastSpawnTime = -SpawnTick;
    }

    private void Update()
    {
        if (CanSpawn)
            Spawn();
    }

    private void Spawn()
    {
        AIAgent agent = Instantiate(spawnPrefab, Lerp(Random.value), transform.rotation).GetComponent<AIAgent>();
        agent.Travel(this, manager.RandomSpawner(this));
        
        _lastSpawnTime = Time.timeSinceLevelLoad;
        _spawned++;
    }

    public void Remove(AIAgent agent)
    {
        --_spawned;
        Destroy(agent.gameObject);
    }

    private void UpdatePoints()
    {
        _points = Array.FindAll(GetComponentsInChildren<Transform>(), child => child != transform);
    }

    private Vector3 Lerp(float t)
    {
        t = Mathf.Clamp(t, 0f, 1f);
        
        float tDistance = 0f;
        for (int i = 0; i < _points.Length - 1; i++)
            tDistance += Vector3.Distance(_points[i].position, _points[i + 1].position);

        tDistance *= t;
        
        Vector3 result = _points[0].position;
        
        for (int i = 0; i < _points.Length - 1 && tDistance > 0.0f; i++)
        {
            Vector3 direction = (_points[i + 1].position - _points[i].position).normalized;
            float distance = Vector3.Distance(_points[i].position, _points[i + 1].position);

            result += direction * Mathf.Min(distance, tDistance);

            tDistance -= distance;
        }
        
        return result;
    }
    
    private Vector3 RawLerp(float t)
    {
        t = Mathf.Clamp(t, 0f, 1f);
        
        if (_points.Length == 1) return _points[0].position;
        
        int index = (int) Mathf.Ceil(t * _points.Length - 1.0f);
        float i = 1.0f / (_points.Length - 1.0f);
        t = (t - (index * i)) / i;

        return Vector3.Lerp(_points[index].position, _points[index + 1].position, t);
    }
    
    public Vector3 ClosestPoint(Vector3 to)
    {
        int nodeIndex = 0;
        float distance = float.MaxValue;

        for (int i = 1; i < _points.Length; i++)
        {
            float d = Vector3.Distance(_points[i].position, to);
            if (d < distance)
            {
                nodeIndex = i;
                distance = d;
            }
        }

        Vector3 result = _points[nodeIndex].position;

        if (nodeIndex > 0)
        {
            Vector3 prevPointOnLine = ClosestPointOnLine(
                to,
                _points[nodeIndex - 1].position,
                _points[nodeIndex].position
            );

            float distanceTo = Vector3.Distance(to, prevPointOnLine);
            
            if (distanceTo < distance)
            {
                distance = distanceTo;
                result = prevPointOnLine;
            }
        }

        if (nodeIndex < _points.Length - 1)
        {
            Vector3 nextPointOnLine = ClosestPointOnLine(
                to,
                _points[nodeIndex + 1].position,
                _points[nodeIndex].position
            );

            float distanceTo = Vector3.Distance(to, nextPointOnLine);

            if (distanceTo < distance)
            {
                distance = distanceTo;
                result = nextPointOnLine;
            }
        }

        return result;
    }

    private static Vector3 ClosestPointOnLine(Vector3 to, Vector3 pointA, Vector3 pointB)
    {
        Vector3 normal = (pointB - pointA).normalized;
        Vector3 centre = (pointA + pointB) / 2f;
        
        Vector3 projection = Vector3.Project(to - pointA, normal) + pointA;
        Vector3 toProjection = Vector3.ClampMagnitude(projection - centre, Vector3.Distance(pointA, pointB) / 2f);
        
        projection = centre + toProjection;
        
        return projection;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (liveUpdate) UpdatePoints();
        if (debugVisualization) Visualise();
        if (debugLabels && _points.Length > 0) Label();
    }

    private void Label()
    {
        Handles.Label(
            Centre + Vector3.up * (visHeight / 2f + visOffset),
            gameObject.name
            );
    }
    
    private void Visualise()
    {
        // Wall between nodes
        Handles.zTest = CompareFunction.Less;
        for (int i = 0; i < _points.Length; i++)
        {
            Handles.CubeHandleCap(0, _points[i].position, transform.rotation, .07f, EventType.Repaint);
            
            if (i == _points.Length - 1) break;
            
            Vector3[] verts = {
                _points[i].position + Vector3.up * visOffset,
                _points[i].position + Vector3.up * (visHeight + visOffset),
                _points[i + 1].position + Vector3.up * (visHeight + visOffset),
                _points[i + 1].position + Vector3.up * visOffset
            };
            
            Handles.DrawSolidRectangleWithOutline(verts, visColor, Color.green);
        }
    }
#endif
}
