using System;
using UnityEditor;
using UnityEngine;

public class Objective : MonoBehaviour
    {
        private GameManager _gameManager;
        private Collider _collider;
        public ParticleSystem activatedPs;
        public AudioSource audioSource;

        private Goal _goal;

        public bool testActivate = false;

        private void Start()
        {
            _gameManager = ServiceLocator.Current.Get<GameManager>();
            _gameManager.OnPreparation += Reset;

            _collider = GetComponent<Collider>();

            _goal = GameObject.FindObjectOfType<Goal>();
            if(_goal) _goal.IncreaseObjectives();
        }

        private void Update()
        {
            if (testActivate)
            {
                Activate();
                testActivate = false;
            }

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
            ShowTextMessage();
            AddScore();
            NotifyGoal();
            PlayParticles();
            PlayAudio();
            DisableCollider();
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
            if(!audioSource.isPlaying) audioSource.Play();
        }
        
        private void StopAudio()
        {
            // Play activation audio (wind chimes)
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
            // Access Scoremanager and update current score accordingly
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
            if (other.CompareTag("Player"))
            {
                Activate();
            }
        }
    }