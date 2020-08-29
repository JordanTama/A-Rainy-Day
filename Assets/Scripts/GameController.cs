using System;
using UnityEngine;


public class GameController : MonoBehaviour
{
    private GameLoopManager _gameLoopManager;

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
    }
}
