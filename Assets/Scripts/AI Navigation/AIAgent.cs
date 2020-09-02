﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

[RequireComponent(typeof(NavMeshAgent))]
public class AIAgent : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private AIManager manager;
    [SerializeField] private NavMeshAgent navMeshAgent;

    [Header("Movement Settings")] 
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float maxSteerSpeed;
    [SerializeField] private float targetRadius;
    [SerializeField] private float slowExponent;

    [Header("Awareness Settings")]
    [SerializeField] private float sightRadius;
    [SerializeField] private float sightAngle;
    [SerializeField] private float sizeRadius;
    [SerializeField] private float obstacleAngleIncrement;

    [Header("Force Proportions")]
    [SerializeField] [Range(0f, 1f)] private float separationProportion;
    [SerializeField] [Range(0f, 1f)] private float alignmentProportion;
    [SerializeField] [Range(0f, 1f)] private float targetProportion;
    [SerializeField] [Range(0f, 1f)] private float cohesionProportion;
    [SerializeField] [Range(0f, 1f)] private float environmentProportion;
    
    [Header("Debug Settings")] 
    [SerializeField] private ForceType debugType;
    [SerializeField] private Color sightColour;

    private AISpawner _spawner;
    private AISpawner _target;
    
    private float _moveMultiplier;
    private float _steerMultiplier;

    private AIAgent[] others = new AIAgent[0];

    private const float ForceRayMultiplier = 2f;
    private const float RayMultiplier = .7f;
    
    
    private enum ForceType { None, All, Total, Separation, Alignment, Cohesion, Target, Environment }


    // MonoBehaviour functions
    private void Update()
    {
        others = GetVisible();
        
        Arbitration(others);
        
        Vector3 separation = Separation(others) * separationProportion;
        Vector3 alignment = Alignment(others) * alignmentProportion;
        Vector3 cohesion = Cohesion(others) * cohesionProportion;
        Vector3 target = Target() * targetProportion;
        Vector3 environment = Environment() * environmentProportion;

        Vector3 totalForce = separation + alignment + target + cohesion + environment;
        totalForce.Normalize();

#if UNITY_EDITOR
        if (debugType == ForceType.Separation)
            Debug.DrawRay(transform.position, separation * ForceRayMultiplier, Color.green);
        
        if (debugType == ForceType.Alignment)
            Debug.DrawRay(transform.position, alignment * ForceRayMultiplier, Color.green);
        
        if (debugType == ForceType.Cohesion)
            Debug.DrawRay(transform.position, cohesion * ForceRayMultiplier, Color.green);
        
        if (debugType == ForceType.Target)
            Debug.DrawRay(transform.position, target * ForceRayMultiplier, Color.green);
        
        if (debugType == ForceType.Environment)
            Debug.DrawRay(transform.position, environment * ForceRayMultiplier, Color.green);
#endif
        
        // _moveMultiplier = Mathf.Pow((Vector3.Dot(totalForce, target) + 1f) * 0.5f, slowExponent);

        Steer(totalForce);
        Move();
        
        CheckExit();
    }

    private void LateUpdate()
    {
        NormalizeProportions();
    }


    // General functions
    public void Travel(AISpawner from, AISpawner to)
    {
        manager.AddNavigator(this);
        
        _target = to;
        _spawner = from;
        
        navMeshAgent.SetDestination(_target.ClosestPoint(transform.position));

        _moveMultiplier = 1.0f;
        _steerMultiplier = 1.0f;
    }

    private AIAgent[] GetVisible()
    {
        List<AIAgent> visible = new List<AIAgent>();

        foreach (AIAgent other in manager.Navigators)
        {
            if (other != this && IsVisible(other))
            {
                visible.Add(other);
            }
        }

        return visible.ToArray();
    }
    
    private void NormalizeProportions()
    {
        float multiplier = 1f / (separationProportion + alignmentProportion + targetProportion + cohesionProportion + environmentProportion);
        if (multiplier == 0f)
        {
            targetProportion = 1f;
        }
        else
        {
            separationProportion *= multiplier;
            alignmentProportion *= multiplier;
            targetProportion *= multiplier;
            cohesionProportion *= multiplier;
            environmentProportion *= multiplier;
        }
    }

    private bool IsVisible(AIAgent other)
    {
        return Vector3.Distance(transform.position, other.transform.position) < sightRadius + other.sizeRadius;
    }

    private void CheckExit()
    {
        if (Vector3.Distance(transform.position, _target.ClosestPoint(transform.position)) > targetRadius) return;
        Clear();
    }

    public void Clear()
    {
        manager.RemoveNavigator(this);
        _spawner.Remove(this);
    }
    
    
    // Motion functions
    private void Arbitration(AIAgent[] others)
    {
    }
    
    private Vector3 Separation(IEnumerable<AIAgent> others)
    {
        Vector3 force = new Vector3();
        foreach (AIAgent other in others)
        {
            float distance = Mathf.Max(
                0f,
                Vector3.Distance(transform.position, other.transform.position) - other.sizeRadius
            ) / sightRadius;
            distance = 1f - distance;
                
            Vector3 direction = (transform.position - other.transform.position).normalized;
            force += direction * distance;
        }
        
        return (force / others.Count()).normalized;
    }

    private Vector3 Alignment(IEnumerable<AIAgent> others)
    {
        Vector3 force = new Vector3();

        foreach (AIAgent other in manager.Navigators)
        {
            if (other != this && Vector3.Distance(transform.position, other.transform.position) < sightRadius + other.sizeRadius)
            {
                force += (other.transform.forward);   
            }
        }

        if (Vector3.Dot(transform.forward, force) < 0) force *= -1f;
        
        return force;
    }
    
    private Vector3 Cohesion(IEnumerable<AIAgent> others)
    {
        Vector3 centre = new Vector3();
        int count = 0;
        
        foreach (AIAgent other in manager.Navigators)
        {
            if (other != this && IsVisible(other))
            {
                centre += other.transform.position;
                ++count;
            }
        }

        if (count == 1) return Vector3.zero;

        centre /= count;
        
        return (centre - transform.position).normalized;
    }
    
    private Vector3 Target()
    {
        if (!_target) return Vector3.zero;
        
        Vector3 force = (navMeshAgent.desiredVelocity).normalized;

        return force;
    }

    private Vector3 Environment()
    {
        Vector3 force = new Vector3();
        float angle = 0f;
        float sign = 1f;
        
        for (int i = 0; i <= (int) (sightAngle / obstacleAngleIncrement); i++)
        {
            Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, sightRadius))
            {
                force -= direction * (1f - (hit.distance / sightRadius));
                
#if UNITY_EDITOR
                if (debugType == ForceType.Environment)
                    Debug.DrawLine(transform.position, hit.point, Color.white);
#endif
            }
            
            angle += (i + 1) * sign * obstacleAngleIncrement;
            sign *= -1f;
        }
        
        return force.normalized;
    }

    private void Steer(Vector3 to)
    {
        if (to.magnitude == 0f) return;
        
        Quaternion toRotation = Quaternion.LookRotation(to, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Mathf.Rad2Deg * maxSteerSpeed * _steerMultiplier * Time.deltaTime * manager.Speed);
    }

    private void Move()
    {
        float speed = maxMoveSpeed * _moveMultiplier * manager.Speed * Time.deltaTime;
        navMeshAgent.Move(transform.forward * speed);
    }


#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (debugType != ForceType.None)
            VisualizeSight();
        
        if (debugType == ForceType.Separation || debugType == ForceType.All)
            VisualizeSeparation();
        
        if (debugType == ForceType.Alignment || debugType == ForceType.All)
            VisualizeAlignment();
        
        if (debugType == ForceType.Cohesion || debugType == ForceType.All)
            VisualizeCohesion();
        
        if (debugType == ForceType.Target || debugType == ForceType.All)
            VisualizeTarget();
    }

    private void DrawSize(Color color)
    {
        Handles.color = color;
            
        Handles.DrawWireArc(transform.position,
            Vector3.up,
            transform.forward,
            360f,
            sizeRadius
        );
    }

    private void VisualizeSeparation()
    {
        foreach (AIAgent other in others)
        {
            Vector3 direction = (other.transform.position - transform.position).normalized;
            float distance = Mathf.Max(0f,
                Vector3.Distance(transform.position, other.transform.position) - other.sizeRadius);

            Handles.color = Color.Lerp(Color.white, Color.red, 1f - (distance / sightRadius));
            
            Handles.DrawLine(
                transform.position,
                transform.position + direction * distance
            );
            
            other.DrawSize(sightColour);
        }
    }

    private void VisualizeAlignment()
    {
        foreach (AIAgent other in others)
        {
            Handles.color = Color.red;
            Handles.DrawLine(other.transform.position, other.transform.position + other.transform.forward * RayMultiplier);
            
            other.DrawSize(sightColour);
        }
    }

    private void VisualizeCohesion()
    {
        Vector3 centre = new Vector3();
        
        foreach (AIAgent other in others)
        {
            centre += other.transform.position;
        }

        centre /= others.Length;

        Handles.color = Color.white;
        
        foreach (AIAgent other in others)
        {
            Handles.DrawLine(other.transform.position, centre);
        }

        Handles.color = Color.red;
        Handles.DrawLine(transform.position, centre);

    }

    private void VisualizeTarget()
    {
        NavMeshPath path = navMeshAgent.path;
        Handles.color = Color.white;
        
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Handles.DrawLine(path.corners[i], path.corners[i + 1]);
        }
    }

    private void VisualizeSight()
    {
        Handles.zTest = CompareFunction.Less;

        Vector3 centre = transform.position;
        Vector3 arcStartVector = Quaternion.AngleAxis(-sightAngle / 2f, Vector3.up) * transform.forward;
        
        // Draw sizeRadius
        DrawSize(Color.white);
        
        // Draw sightRadius
        Handles.color = Color.white;
        Handles.DrawWireArc(centre, Vector3.up, arcStartVector, 360f, sightRadius);
        
        // Draw sightAngle
        Handles.color = sightColour;
        Handles.DrawSolidArc(centre, Vector3.up, arcStartVector, sightAngle, sightRadius);
    }

#endif
}