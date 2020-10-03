using System;
using UnityEngine;


public class GameLoopController : MonoBehaviour
{
    private GameLoopManager _gameLoopManager;
    
    private MainUIController _mainUiController;
    
    private void Awake()
    {
        _mainUiController = GetComponentInParent<MainUIController>();
    }

    private void Start()
    {
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
    }

    public void Execute()
    {
        _gameLoopManager.Execute();
    }

    public void Reset()
    {
        _gameLoopManager.SoftResetLevel();
    }

    public void Restart()
    {
        _gameLoopManager.RestartLevel();
        
        if(_mainUiController) _mainUiController.PlayMenuButtonAudio();
    }
}
