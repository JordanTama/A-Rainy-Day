using System;
using UnityEngine;
using UnityEngine.UI;


public class ExecuteButtonState : MonoBehaviour
{
    private GameManager _gameManager;
    private Button _myButton;

    private void Start()
    {
        _gameManager = ServiceLocator.Current.Get<GameManager>();
        _myButton = GetComponent<Button>();
        _gameManager.OnPreparation += EnableInteraction;
        _gameManager.OnExecution += DisableInteraction;
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
}
