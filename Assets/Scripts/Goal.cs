using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private GameLoopManager _gameLoopManager;
    private ScoreManager _scoreManager;

    private int playerPoints = 10;
    private int npcPoints = 1;
    private int _pointsToAdd = 0;
    
    private Collider _collider;
    public ParticleSystem openPs;
    [SerializeField] private AudioSource goalOpenAudioSource;
    [SerializeField] private AudioSource goalEnteredAudioSource;

    public TMP_Text goalText;
    
    public AudioClip goalEnteredClip;
    public AudioClip goalOpenClip;
    public AudioClip playerEnterGoalCip;
    
    private int _objectiveCount = 0;
    private int _objectiveActivated = 0;

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        if (goalOpenAudioSource && goalOpenClip) goalOpenAudioSource.clip = goalOpenClip;
        if (goalEnteredAudioSource && goalEnteredClip) goalEnteredAudioSource.clip = goalEnteredClip;

    }

    private void Start()
    {
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        _scoreManager = ServiceLocator.Current.Get<ScoreManager>();
        _gameLoopManager.OnPreparation += Reset;

        
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
            if (_objectiveCount > 0)
            {
                PlayOpenAudio();
            }
            
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
        if(goalEnteredAudioSource) goalEnteredAudioSource.clip = goalEnteredClip;
        CloseGoal();
        ResetActiveObjectives();

    }

    private void StopAudio()
    {
        if (goalEnteredAudioSource)
        {
            if(goalEnteredAudioSource.isPlaying) goalEnteredAudioSource.Stop();
        }
        
        if (goalOpenAudioSource)
        {
            if(goalOpenAudioSource.isPlaying) goalOpenAudioSource.Stop();
        }
    }

    
    private void Entered()
    {
        AddScore();
        PlayEnteredAudio();
    }

    private void LevelComplete()
    {
        CloseGoal();
        PlayLevelCompleteAudio();
        _gameLoopManager.Complete();
    }

    private void PlayEnteredAudio()
    {
        if (!goalEnteredAudioSource) return;
        if (!goalEnteredAudioSource.clip) return;
        if(goalEnteredAudioSource.isPlaying) goalEnteredAudioSource.Stop();
        goalEnteredAudioSource.Play();
    }

    private void PlayLevelCompleteAudio()
    {
        if (!goalEnteredAudioSource) return;
        goalEnteredAudioSource.clip = playerEnterGoalCip;
        PlayEnteredAudio();
    }

    private void PlayOpenAudio()
    {
        if (!goalOpenAudioSource) return;
        if(goalOpenAudioSource.isPlaying) goalOpenAudioSource.Stop();
        goalOpenAudioSource.Play();
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
            
            if (other.CompareTag("Player"))
            {
                LevelComplete();
            }
            
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