using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class NavigationGate : NavigationNode
{
    [Header("Gate Geometry")]
    [SerializeField] private float gatePadding;

    [Header("Debug Settings")]
    [SerializeField] private Color pathColour;
    [SerializeField] private float pathHeight;
    [SerializeField] private float pathOffset;

    private Transform[] _points;
    [SerializeField] private NavigationNode[] _connectedNodes;


    public NavigationNode[] ConnectedNodes => _connectedNodes.Clone() as NavigationNode[];
    public override Vector3 Centre => Lerp(0.5f);
    
    public override void UpdateNode() => _points = Array.FindAll(GetComponentsInChildren<Transform>(), child => child != transform);

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
    
    public override Vector3 ClosestPoint(Vector3 to)
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

    private Vector3 ClosestPointOnLine(Vector3 to, Vector3 pointA, Vector3 pointB)
    {
        Vector3 normal = (pointB - pointA).normalized;
        Vector3 projection = Vector3.Project(to - pointA, normal) + pointA;
        float projectionDistance = Vector3.Distance(projection, to);
        
        if (Vector3.Distance(to, pointA) < projectionDistance) return pointA;
        if (Vector3.Distance(to, pointB) < projectionDistance) return pointB;
        
        return projection;
    }

    private Vector3 LerpPadded()
    {
        throw new System.NotImplementedException();
    }
    

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (debugVisualisation) Visualise();
        if (debugLabels && _points.Length > 0) Label();
    }

    protected override void Label()
    {
        Handles.Label(
            Centre + Vector3.up * (pathHeight / 2f + pathOffset),
            gameObject.name
            );
    }
    
    protected override void Visualise()
    {
        // Wall between nodes
        Handles.zTest = CompareFunction.Less;
        for (int i = 0; i < _points.Length; i++)
        {
            Handles.CubeHandleCap(0, _points[i].position, Quaternion.identity, .07f, EventType.Repaint);
            
            if (i == _points.Length - 1) break;
            
            Vector3[] verts = {
                _points[i].position + Vector3.up * pathOffset,
                _points[i].position + Vector3.up * (pathHeight + pathOffset),
                _points[i + 1].position + Vector3.up * (pathHeight + pathOffset),
                _points[i + 1].position + Vector3.up * pathOffset
            };
            
            Handles.DrawSolidRectangleWithOutline(verts, pathColour, Color.green);
        }

        foreach (NavigationNode other in _connectedNodes)
        {
            Handles.DrawLine(
                Centre,
                other.Centre
            );
        }
    }
#endif
}
