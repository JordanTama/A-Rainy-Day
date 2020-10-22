using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public Action OnLevelReady;
    
    
    public GameLoopManager()
    {
        gameState = GameState.Preparation;

        SceneManager.activeSceneChanged += RestartLevel;
        // OnLevelReady += RestartLevel;
    }

    private void RestartLevel(Scene arg0, Scene arg1)
    {
        if (arg1.name.Equals("TransitionScene")) return;
        RestartLevel();
    }

    private void ChangeState(GameState newState)
    {
        gameState = newState;
    }

    public void RestartLevel()
    {
        OnRestart?.Invoke();
        SoftResetLevel();
        
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
        Debug.Log("level complete");
        ChangeState(GameState.Complete);
        OnComplete?.Invoke();
    }
}
