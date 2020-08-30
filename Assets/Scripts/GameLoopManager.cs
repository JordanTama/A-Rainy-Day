using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class GameLoopManager:IGameService
{
    public enum GameState { Preparation,Execution,Complete};
    public GameState gameState;
    public Action OnPreparation;
    public Action OnExecution;
    public Action OnRestart;
    public Action OnComplete;
    
    
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
        SoftResetLevel();
        OnRestart?.Invoke();
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

    public void Complete()
    {
        ChangeState(GameState.Complete);
        OnComplete?.Invoke();
    }
}
