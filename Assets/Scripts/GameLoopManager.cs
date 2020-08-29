using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameLoopManager:IGameService
{
    public enum GameState { Preparation,Execution};
    public GameState gameState;
    public Action OnPreparation;
    public Action OnExecution;
    
    
    public GameLoopManager()
    {
        gameState = GameState.Preparation;
    }

    private void ChangeState(GameState newState)
    {
        gameState = newState;
    }

    public void RestartLevel()
    {
        // Reload the current active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SoftResetLevel()
    {
        ChangeState(GameState.Preparation);
        OnPreparation?.Invoke();
    }

    public void Execute()
    {
        ChangeState(GameState.Execution);
        OnExecution?.Invoke();
    }
}
