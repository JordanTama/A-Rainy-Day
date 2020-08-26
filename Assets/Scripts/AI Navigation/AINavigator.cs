using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class AINavigator : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private AIManager manager;
    
    [Header("Movement Settings")] 
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float maxSteerSpeed;

    [Header("Awareness Settings")]
    [SerializeField] private float sightRadius;
    [SerializeField] private float sightAngle;
    [SerializeField] private float sizeRadius;

    [Header("Avoidance Settings")]
    [SerializeField] private float raycastStep;
    [SerializeField] private LayerMask avoidanceMask;

    [Header("Debug Settings")] 
    [SerializeField] private bool debugVisualization;
    [SerializeField] private Color sightColour;

    private float _moveMultiplier = 1.0f;
    private float _steerMultiplier = 1.0f;

    private void Awake()
    {
        manager.AddNavigator(this);
    }

    private void Update()
    {
        Vector3 avoidance = AvoidanceForce();
        Vector3 alignment = AlignmentForce();

        Vector3 totalForce = avoidance + alignment;
        totalForce.Normalize();
        
        Steer(totalForce);
        Move();
    }


    private Vector3 AvoidanceForce()
    {
        Vector3 force = new Vector3();

        foreach (AINavigator other in manager.Navigators)
        {
            if (other != this && Vector3.Distance(transform.position, other.transform.position) < sightRadius + other.sizeRadius)
            {
                force += (transform.position - other.transform.position).normalized;   
            }
        }
        
        return force.normalized;
    }

    private Vector3 AlignmentForce()
    {
        Vector3 force = new Vector3();

        foreach (AINavigator other in manager.Navigators)
        {
            if (other != this && Vector3.Distance(transform.position, other.transform.position) < sightRadius + other.sizeRadius)
            {
                force += (other.transform.forward);   
            }
        }

        if (Vector3.Dot(transform.forward, force) < 0) force *= -1f;
        
        return force;
    }
    
    private void Steer(Vector3 to)
    {
        if (to.magnitude == 0f) return;
        
        Quaternion toRotation = Quaternion.LookRotation(to, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Mathf.Rad2Deg * maxSteerSpeed * _steerMultiplier * Time.deltaTime);
    }

    private void Move()
    {
        transform.Translate(transform.forward * (maxMoveSpeed * _moveMultiplier * Time.deltaTime), Space.World);
    }


#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (debugVisualization) Visualize();
    }

    private void Visualize()
    {
        Handles.zTest = CompareFunction.Less;

        Vector3 centre = transform.position;
        Vector3 arcStartVector = Quaternion.AngleAxis(-sightAngle / 2f, Vector3.up) * transform.forward;
        
        // Draw sizeRadius
        Handles.color = Color.white;
        Handles.DrawWireArc(centre, Vector3.up, arcStartVector, 360f, sizeRadius);
        
        // Draw sightRadius
        Handles.color = Color.white;
        Handles.DrawWireArc(centre, Vector3.up, arcStartVector, 360f, sightRadius);
        
        // Draw sightAngle
        Handles.color = sightColour;
        Handles.DrawSolidArc(centre, Vector3.up, arcStartVector, sightAngle, sightRadius);
    }

#endif
}
