using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Interactions;

public class ScoreManager:IGameService
{
    private GameManager _gameManager;
    private int _currentScore;

    public ScoreManager(GameManager GameMan)
    {
        _gameManager = GameMan;
        _currentScore = 0;
        _gameManager.OnPreparation += ResetScore;
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
