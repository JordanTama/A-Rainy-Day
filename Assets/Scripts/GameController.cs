using System;
using UnityEngine;


public class GameController : MonoBehaviour
{
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = ServiceLocator.Current.Get<GameManager>();
    }

    public void Execute()
    {
        _gameManager.Execute();
    }

    public void Reset()
    {
        _gameManager.SoftResetLevel();
    }

    public void Restart()
    {
        _gameManager.RestartLevel();
    }
}
