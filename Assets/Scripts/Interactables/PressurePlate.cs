using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class PressurePlate : InteractableController
{
    public int currentCount;
    public int goalCount;
    
    public Action onCountChange;

    protected new void Start()
    {
        base.Start();
        _gameLoopManager.OnPreparation += ResetInteractable;
        ResetInteractable();


    }

    protected override void ResetInteractable()
    {
        currentCount = 0;
        onCountChange?.Invoke();
        base.ResetInteractable();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            AddCount();
            if (IsCountAtGoal() && currentState == defaultState)
            {
                ChangeState(nextState);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            if (IsCountAtGoal() && currentState!=defaultState)
            {
                ChangeState(defaultState);
            }
            SubtractCount();
            
        }
    }

    private void AddCount()
    {
        currentCount ++;
        onCountChange?.Invoke();
        

    }
    
    private void SubtractCount()
    {

        if (currentCount>0)
        {
            currentCount --;
            onCountChange?.Invoke();
        }

    }

    private bool IsCountAtGoal()
    {
        return currentCount == goalCount;
    }

    private void OnDestroy()
    {
        _gameLoopManager.OnPreparation -= ResetInteractable;
    }
}
