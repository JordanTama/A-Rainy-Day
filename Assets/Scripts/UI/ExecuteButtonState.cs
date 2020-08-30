using System;
using UnityEngine;
using UnityEngine.UI;


public class ExecuteButtonState : MonoBehaviour
{
    private GameLoopManager _gameLoopManager;
    private Button _myButton;

    private void Start()
    {
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        _myButton = GetComponent<Button>();
        _gameLoopManager.OnPreparation += EnableInteraction;
        _gameLoopManager.OnExecution += DisableInteraction;
        _gameLoopManager.OnComplete += DisableInteraction;
        EnableInteraction();
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
        _gameLoopManager.OnPreparation -= EnableInteraction;
        _gameLoopManager.OnExecution -= DisableInteraction;
        _gameLoopManager.OnComplete -= DisableInteraction;
    }
}
