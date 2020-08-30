using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private GameManager _gameManager;
    private ScoreManager _scoreManager;

    public int playerPoints = 10;
    public int npcPoints = 1;
    private int _pointsToAdd = 0;
    
    public Collider _collider;
    public ParticleSystem openPs;
    public AudioSource openAs;

    public TMP_Text goalText;

    private int objectiveCount = 0;
    private int objectiveActivated = 0;
    
    private void Start()
    {
        _gameManager = ServiceLocator.Current.Get<GameManager>();
        _scoreManager = ServiceLocator.Current.Get<ScoreManager>();
        _gameManager.OnPreparation += Reset;

        _collider = GetComponent<Collider>();
        Reset();
    }
    

    public void IncreaseObjectivesCount()
    {
        objectiveCount++;
        OpenGoal();
    }

    private void ResetActiveObjectives()
    {
        objectiveActivated = 0;
        OpenGoal();
    }

    public void IncreaseActiveCount()
    {
        objectiveActivated++;
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
            goalText.text = "" + objectiveActivated + "/" + objectiveCount;
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
        return objectiveActivated >= objectiveCount;
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
            _pointsToAdd = DeterminePoints(other.tag);
            Entered();
        }
    }

    private int DeterminePoints(string otherTag)
    {
        return otherTag.Equals("Player") ? playerPoints : npcPoints;
    }
}