using System;
using UnityEditor;
using UnityEngine;

public class Objective : MonoBehaviour
    {
        private GameLoopManager _gameLoopManager;
        private ScoreManager _scoreManager;
        private Collider _collider;
        private int _points = 5;
        public ParticleSystem activatedPs;
        private AudioSource audioSource;
        [SerializeField] private AudioClip activatedAudioClip;
        [SerializeField] private float audioVolume = 0.5f;

        [SerializeField] private Goal _goal;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if(audioSource && activatedAudioClip)
            {
                audioSource.clip = activatedAudioClip;
                audioSource.volume = audioVolume;
            }
        }

        private void Start()
        {
            _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
            _scoreManager = ServiceLocator.Current.Get<ScoreManager>();
            _gameLoopManager.OnPreparation += Reset;

            _collider = GetComponent<Collider>();

            _goal = GameObject.FindObjectOfType<Goal>();
            if(_goal) _goal.IncreaseObjectivesCount();
        }

        private void Reset()
        {
            HideTextMessage();
            StopParticles();
            StopAudio();
            EnableCollider();

        }

        private void Activate()
        {
            DisableCollider();
            ShowTextMessage();
            AddScore();
            NotifyGoal();
            PlayParticles();
            PlayAudio();
            
        }

        private void NotifyGoal()
        {
            if (!_goal) return;
            _goal.IncreaseActiveCount();
        }

        private void PlayAudio()
        {
            // Play activation audio (wind chimes)
            if (!audioSource) return;
            if (!audioSource.clip) return;
            if(audioSource.isPlaying) StopAudio();
            audioSource.Play();
        }
        
        private void StopAudio()
        {
            // stop activation audio (wind chimes)
            if (!audioSource) return;
            if(audioSource.isPlaying) audioSource.Stop();
        }

        private void EnableCollider()
        {
            _collider.enabled = true;
        }
        
        private void DisableCollider()
        {
            _collider.enabled = false;
        }

        private void PlayParticles()
        {
            if (!activatedPs) return;
            if(!activatedPs.isPlaying) activatedPs.Play();
        }
        
        private void StopParticles()
        {
            if (!activatedPs) return;
            if (activatedPs.isPlaying)
            {
                activatedPs.Stop();
                activatedPs.Clear();
            }
        }

        private void AddScore()
        {
            _scoreManager.AddScore(_points);
        }

        private void ShowTextMessage()
        {
            // Activate text message
        }
        
        private void HideTextMessage()
        {
            // Hide/Delete text Message
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")||other.CompareTag("NPC"))
            {
                Activate();
            }
        }
    }