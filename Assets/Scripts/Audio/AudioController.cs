using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    protected AudioManager _audioManager;
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected AudioClip _currentAudioClip;
    protected GameLoopManager _gameLoopManager;
    [SerializeField] protected AudioMixer _audioMixer;

    protected virtual void Awake()
    {
        _audioManager = ServiceLocator.Current.Get<AudioManager>();
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
    }
    protected virtual void Start()
    {
        
    }

    public virtual void Play()
    {
        
    }

    public virtual void Pause()
    {
        
    }

    public virtual void Stop()
    {
        
    }

    protected virtual void Register()
    {
        _audioManager.OnAudioStop += Stop;
        _audioManager.OnAudioPause += Pause;
    }

    protected virtual void Deregister()
    {
        _audioManager.OnAudioStop -= Stop;
        _audioManager.OnAudioPause -= Pause;
    }

    protected void OnDestroy()
    {
        Deregister();
    }

    protected void OnEnable()
    {
        Register();
    }

    protected void OnDisable()
    {
        Deregister();
    }
}
