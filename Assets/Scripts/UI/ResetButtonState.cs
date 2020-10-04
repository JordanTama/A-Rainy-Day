using System;
using UnityEngine;
using UnityEngine.UI;


public class ResetButtonState : MonoBehaviour
{
    private GameLoopManager _gameLoopManager;
    private Button _myButton;
    

    private void Start()
    {
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        _myButton = GetComponent<Button>();
        _gameLoopManager.OnPreparation += DisableInteraction;
        _gameLoopManager.OnExecution += EnableInteraction;
        _gameLoopManager.OnComplete += DisableInteraction;
        DisableInteraction();
    }

    private void EnableInteraction()
    {
        _myButton.interactable = true;
    }
    
    private void DisableInteraction()
    {
        _myButton.interactable = false;
    }

    private void OnDestroy()
    {
        _gameLoopManager.OnPreparation -= DisableInteraction;
        _gameLoopManager.OnExecution -= EnableInteraction;
        _gameLoopManager.OnComplete -= DisableInteraction;
    }
}