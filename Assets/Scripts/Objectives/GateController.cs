using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

public class GateController : MonoBehaviour
{
    [SerializeField] private Gate[] goals = new Gate[0];
    [SerializeField] private Gate[] objectives = new Gate[0];

    private int _completed = 0;
    

    private float Ratio => objectives.Length == 0 ? 1 : (float) _completed / objectives.Length;


    private void Awake()
    {
        AssignFromScene();
        Reset();
    }

    [ContextMenu("Auto Assign Gates")]
    public void AssignFromScene()
    {
        List<Gate> foundGoals = new List<Gate>();
        foreach (Goal goalInScene in FindObjectsOfType<Goal>())
        {
            if (goalInScene.gate && !foundGoals.Contains(goalInScene.gate))
                foundGoals.Add(goalInScene.gate);
        }
        
        List<Gate> foundObjectives = new List<Gate>();
        foreach (Objective objectiveInScene in FindObjectsOfType<Objective>())
        {
            if (objectiveInScene.gate && !foundObjectives.Contains(objectiveInScene.gate))
                foundObjectives.Add(objectiveInScene.gate);
        }

        goals = foundGoals.ToArray();
        objectives = foundObjectives.ToArray();
    }
    
    public void Trigger(Gate gate)
    {
        bool inArray = false;
        foreach (Gate objective in objectives)
        {
            if (objective != gate) continue;
            
            objective.Trigger();
            _completed++;
            inArray = true;
            break;
        }

        if (!inArray) return;

        foreach (Gate goal in goals)
        {
            goal.SetFill(Ratio);
        }
    }

    [ContextMenu("Reset All")]
    public void Reset()
    {
        _completed = 0;
        
        foreach (Gate objective in objectives)
            objective.Reset();
        
        foreach (Gate goal in goals)
            goal.SetFill(Ratio, false);
    }
}
