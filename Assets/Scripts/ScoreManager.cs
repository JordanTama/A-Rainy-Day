using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Interactions;

public class ScoreManager:IGameService
{
    private GameLoopManager _gameLoopManager;
    private int _currentScore;

    public ScoreManager(GameLoopManager gameLoopMan)
    {
        _gameLoopManager = gameLoopMan;
        _currentScore = 0;
        _gameLoopManager.OnPreparation += ResetScore;
    }

    public void AddScore(int points)
    {
        _currentScore += points;
    }

    public void SubtractScore(int points)
    {
        _currentScore -= points;
    }

    public int GetScore()
    {
        return _currentScore;
    }

    private void ResetScore()
    {
        _currentScore = 0;
    }
}
