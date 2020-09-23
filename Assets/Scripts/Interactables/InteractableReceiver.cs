using System;
using UnityEngine;


public abstract class InteractableReceiver : MonoBehaviour
{
    public InteractableController interactableController;

    protected void Awake()
    {
        
    }

    protected void Start()
    {
        if (interactableController)
        {
            interactableController.OnInteractableStateChange += ChangeState;
            interactableController.OnInteractableReset += ResetState;
            
        }
    }

    protected virtual void ChangeState()
    {
        
    }

    protected virtual void ResetState()
    {
        
    }

    protected void OnDestroy()
    {
        if (interactableController)
        {
            interactableController.OnInteractableStateChange -= ChangeState;
            interactableController.OnInteractableReset -= ResetState;
        }
        
    }
}
