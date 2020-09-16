using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractableController : MonoBehaviour
{
    public Action OnInteractableStateChange;

    protected InteractableManager interactableManager;
    protected InputManager inputManager;
    protected CameraManager cameraManager;
    protected GameLoopManager _gameLoopManager;

    [SerializeField] protected int numStates;
    [SerializeField] protected int currentState;
    protected int nextState;
    [SerializeField] protected int defaultState;
    private bool isChangeState;
    

    protected void Start()
    {
        interactableManager = ServiceLocator.Current.Get<InteractableManager>();
        inputManager = ServiceLocator.Current.Get<InputManager>();
        cameraManager = ServiceLocator.Current.Get<CameraManager>();
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        
        interactableManager.OnInteractableSelect+=InteractableSelect;
        interactableManager.OnInteractableDeselect+=InteractableDeselect;

        _gameLoopManager.OnPreparation += ResetInteractable;

        ResetInteractable();
    }

    protected virtual void ResetInteractable()
    {
        ChangeState(defaultState);
    }

    protected void ChangeState(int newState)
    {
        currentState = newState;
        SetNextState();
        OnInteractableStateChange?.Invoke();
        interactableManager.OnInteractableStateChange?.Invoke();
    }

    protected void SetNextState()
    {
        nextState = (currentState + 1) % numStates;
    }

    protected void InteractableSelect(GameObject obj)
    {
        if (obj == gameObject)
        {
            ChangeState(nextState);
        }
    }
    
    protected void InteractableDeselect()
    {
        // Cancel highlighting
    }

    protected void OnDestroy()
    {
        interactableManager.OnInteractableSelect-=InteractableSelect;
        interactableManager.OnInteractableDeselect-=InteractableDeselect;

        _gameLoopManager.OnPreparation -= ResetInteractable;
    }
}
