using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class NavigationNode : MonoBehaviour
{
    [Header("Debug Settings")]
    [SerializeField] protected bool debugVisualisation;
    [SerializeField] protected bool debugLabels;

    public virtual Vector3 Centre => transform.position;
    
    
    public virtual Vector3 ClosestPoint(Vector3 to)
    {
        return transform.position;
    }

    public virtual void UpdateNode() { }
    
    
#if UNITY_EDITOR
    protected virtual void Label()
    {
        Handles.Label(
            transform.position,
            gameObject.name
        );
    }

    protected virtual void Visualise()
    {
        Handles.CubeHandleCap(
            0,
            transform.position,
            Quaternion.identity,
            0.7f,
            EventType.Repaint
        );
    }
#endif
}
