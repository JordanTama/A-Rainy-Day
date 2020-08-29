using System;
using UnityEngine;
using UnityEngine.UI;


public class ResetButtonState : MonoBehaviour
{
    private GameManager _gameManager;
    private Button _myButton;

    private void Start()
    {
        _gameManager = ServiceLocator.Current.Get<GameManager>();
        _myButton = GetComponent<Button>();
        _gameManager.OnPreparation += DisableInteraction;
        _gameManager.OnExecution += EnableInteraction;
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
}