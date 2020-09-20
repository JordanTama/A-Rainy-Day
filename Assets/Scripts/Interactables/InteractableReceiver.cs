using System;
using UnityEngine;


public abstract class InteractableReceiver : MonoBehaviour
{
    public InteractableController interactableController;
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
        interactableController.OnInteractableStateChange -= ChangeState;
        interactableController.OnInteractableReset -= ResetState;
    }
}
