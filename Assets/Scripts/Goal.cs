using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private GameManager _gameManager;
    public Collider _collider;
    public ParticleSystem OpenPs;
    public AudioSource openAS;

    public TMP_Text goalText;

    [SerializeField] private int objectiveCount = 0;
    [SerializeField] private int objectiveActivated = 0;
    
    private void Start()
    {
        _gameManager = ServiceLocator.Current.Get<GameManager>();
        _gameManager.OnPreparation += Reset;

        _collider = GetComponent<Collider>();
        Reset();
    }

    public void IncreaseObjectives()
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
        return objectiveActivated == objectiveCount;
    }

    private void Reset()
    {
        CloseGoal();
        ResetActiveObjectives();

    }

    private void StopAudio()
    {
        if (!openAS) return;
        if(openAS.isPlaying) openAS.Stop();
    }



    private void Entered()
    {
        AddScore();
        PlayAudio();
    }

    private void PlayAudio()
    {
        if (!openAS) return;
        if(!openAS.isPlaying) openAS.Play();
    }

    private void PlayParticles()
    {
        if (!OpenPs) return;
        if(!OpenPs.isPlaying) OpenPs.Play();
    }
    
    private void StopParticles()
    {
        if (!OpenPs) return;
        if (OpenPs.isPlaying)
        {
            OpenPs.Stop();
            OpenPs.Clear();
        }
    }

    private void AddScore()
    {
        // Add score from player crossing goal, modify for an npc as well
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) Entered();
    }
}