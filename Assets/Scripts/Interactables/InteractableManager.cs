using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableManager : IGameService
{
    public GameObject CurrentInteractable { get; private set; }
    public Action<GameObject> OnInteractableSelect;
    public Action OnInteractableDeselect;
    public Action OnInteractableStateChange;

    public Action OnRebakeMesh;

    private CameraManager cameraManager;
    private InputManager inputManager;
    private GameLoopManager _gameLoopManager;

    private bool _canInteract;


    public InteractableManager(CameraManager camMan, InputManager input, GameLoopManager gameLoopMan)
    {
        cameraManager = camMan;
        cameraManager.OnCameraRaycastHit += InteractableSelect;

        inputManager = input;
        inputManager.P_LeftClick.canceled += InteractableDeselect;

        _gameLoopManager = gameLoopMan;
        _gameLoopManager.OnPreparation += EnableInteract;
        _gameLoopManager.OnExecution += EnableInteract;
        _gameLoopManager.OnComplete += DisableInteract;
        EnableInteract();

        SceneManager.activeSceneChanged += SceneChanged;
    }

    private void SceneChanged(Scene arg0, Scene arg1)
    {
        CurrentInteractable = null;
        _canInteract = true;
    }

    private void InteractableDeselect(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (CurrentInteractable == null)
            return;

        CurrentInteractable.layer = 0;
        CurrentInteractable = null;
        OnInteractableDeselect?.Invoke();
    }

    public void InteractableSelect(RaycastHit[] hits)
    {
        if (!_canInteract)
            return;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Interactable") && CurrentInteractable == null)
            {
                CurrentInteractable = hit.collider.gameObject;
                CurrentInteractable.layer = 2;
                OnInteractableSelect?.Invoke(CurrentInteractable);
                break;
            }
        }
    }

    public void NewInteractableState()
    {
        OnInteractableStateChange?.Invoke();
    }

    private void EnableInteract()
    {
        _canInteract = true;
    }
    
    private void DisableInteract()
    {
        _canInteract = false;
    }
    
    

}
