using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractableController : MonoBehaviour
{
    public Action OnInteractableStateChange;
    
    private InteractableManager interactableManager;
    private InputManager inputManager;
    private CameraManager cameraManager;
    private GameLoopManager _gameLoopManager;

    [SerializeField] private int numStates;
    [SerializeField] private int currentState;
    private int nextState;
    [SerializeField] private int defaultState;
    private bool isChangeState;
    

    private void Start()
    {
        interactableManager = ServiceLocator.Current.Get<InteractableManager>();
        inputManager = ServiceLocator.Current.Get<InputManager>();
        cameraManager = ServiceLocator.Current.Get<CameraManager>();
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        
        interactableManager.OnInteractableSelect+=InteractableSelect;
        interactableManager.OnInteractableDeselect+=InteractableDeselect;

        _gameLoopManager.OnRestart += ResetInteractable;

        ResetInteractable();
    }

    private void ResetInteractable()
    {
        ChangeState(defaultState);
    }

    private void ChangeState(int newState)
    {
        currentState = newState;
        SetNextState();
        OnInteractableStateChange?.Invoke();
        interactableManager.OnInteractableStateChange?.Invoke();
    }

    private void SetNextState()
    {
        nextState = (currentState + 1) % numStates;
    }

    private void InteractableSelect(GameObject obj)
    {
        if (obj == gameObject)
        {
            ChangeState(nextState);
        }
    }
    
    private void InteractableDeselect()
    {
        // Cancel highlighting
    }
}
