using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : IGameService
{
    private GameLoopManager _gameLoopManager;
    public Action OnAudioPlay; 
    public Action OnAudioPause; 
    public Action OnAudioStop;

    public AudioMixer Mixer { get; private set; }
    
    public AudioManager(GameLoopManager gameLoopManager)
    {
        _gameLoopManager = gameLoopManager;

        Mixer = Resources.Load<AudioMixer>("MasterMixer");
    }

    public void PlayAllAudio()
    {
        OnAudioPlay?.Invoke();
    }
    
    public void PauseAllAudio()
    {
        OnAudioPause?.Invoke();
    }

    public void StopAllAudio()
    {
        OnAudioStop?.Invoke();
    }
}
