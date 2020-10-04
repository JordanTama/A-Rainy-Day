using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : IGameService
{
    private GameLoopManager _gameLoopManager;
    public Action OnAudioPlay; 
    public Action OnAudioPause; 
    public Action OnAudioStop; 
    
    public AudioManager(GameLoopManager gameLoopManager)
    {
        _gameLoopManager = gameLoopManager;
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
