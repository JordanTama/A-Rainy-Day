using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private GameLoopManager _gameLoopManager;
    private ScoreManager _scoreManager;

    public int playerPoints = 10;
    public int npcPoints = 1;
    private int _pointsToAdd = 0;
    
    public Collider _collider;
    public ParticleSystem openPs;
    public AudioSource openAs;

    public TMP_Text goalText;

    private int _objectiveCount = 0;
    private int _objectiveActivated = 0;

    private void Start()
    {
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        _scoreManager = ServiceLocator.Current.Get<ScoreManager>();
        _gameLoopManager.OnPreparation += Reset;

        _collider = GetComponent<Collider>();
        Reset();
    }
    

    public void IncreaseObjectivesCount()
    {
        _objectiveCount++;
        OpenGoal();
    }

    private void ResetActiveObjectives()
    {
        _objectiveActivated = 0;
        OpenGoal();
    }

    public void IncreaseActiveCount()
    {
        _objectiveActivated++;
        OpenGoal();
    }

    private void OpenGoal()
    {
        if (IsOpen())
        {
            PlayParticles();
            _collider.enabled = true;
        }
        else
        {
            CloseGoal();
        }

        if (goalText)
        {
            goalText.alpha = IsOpen() ? 0 : 1;
            goalText.text = "" + _objectiveActivated + "/" + _objectiveCount;
        }
    }

    private void CloseGoal()
    {
        StopParticles();
        StopAudio();
        _collider.enabled = false;
    }

    private bool IsOpen()
    {
        return _objectiveActivated >= _objectiveCount;
    }

    private void Reset()
    {
        CloseGoal();
        ResetActiveObjectives();

    }

    private void StopAudio()
    {
        if (!openAs) return;
        if(openAs.isPlaying) openAs.Stop();
    }

    
    private void Entered()
    {
        AddScore();
        PlayAudio();
    }

    private void LevelComplete()
    {
        CloseGoal();
        _gameLoopManager.Complete();
    }

    private void PlayAudio()
    {
        if (!openAs) return;
        if(!openAs.isPlaying) openAs.Play();
    }

    private void PlayParticles()
    {
        if (!openPs) return;
        if(!openPs.isPlaying) openPs.Play();
    }
    
    private void StopParticles()
    {
        if (!openPs) return;
        if (openPs.isPlaying)
        {
            openPs.Stop();
            openPs.Clear();
        }
    }

    private void AddScore()
    {
        // Add score from player crossing goal, modify for an npc as well
        _scoreManager.AddScore(_pointsToAdd);
        _pointsToAdd = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            if (other.CompareTag("Player"))
            {
                LevelComplete();
            }
            _pointsToAdd = DeterminePoints(other.tag);
            Entered();
        }
    }

    private int DeterminePoints(string otherTag)
    {
        return otherTag.Equals("Player") ? playerPoints : npcPoints;
    }

    private void OnDestroy()
    {
        _gameLoopManager.OnPreparation -= Reset;
    }
}