﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SoundtrackController : AudioController
{
    [SerializeField] private AudioClip[] _chapterAudioClips = new AudioClip[6];
    [SerializeField] private AudioSource[] _chapterAudioSources = new AudioSource[6];
    [SerializeField] private int _levelNum=0;
    [SerializeField] private int defaultLevelNum;
    [SerializeField] private float musicVolume = 1f;
    protected override void Awake()
    {
        base.Awake();
        GetLevelNum();
        AssignClips();
        
    }

    protected override void Start()
    {
        base.Start();
        Play();
    }

    public override void Play()
    {
        // Plays all audio sources, but mutes the ones that arent required atm, but has them ready to be unmuted if needed
        
        base.Play();
        if (_levelNum < 1 || _levelNum > 6) return;
        for (int i=0;i<_chapterAudioSources.Length;i++)
        {
            var audioSource = _chapterAudioSources[i];
            if (!audioSource)
            {
                print("Audio source missing for level "+i);
                return;
            }
            
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            audioSource.volume = musicVolume;

            if (i >= _levelNum)
            {
                audioSource.DOFade(0f, 1f);
            }
            else
            {
                audioSource.DOFade(musicVolume, 1f);
            }
            
            audioSource.mute = i >= _levelNum;

        }
    }

    public override void Pause()
    {
        base.Pause();
        foreach (var v in _chapterAudioSources)
        {
            if (!v) return;
            if (v.isPlaying)
            {
                v.Pause();
            }
        }
    }

    public override void Stop()
    {
        base.Stop();
        foreach (var v in _chapterAudioSources)
        {
            if (!v) return;
            if (v.isPlaying)
            {
                v.Stop();
            }
        }
    }

    public void IncrementLevelNumber()
    {
        
        _levelNum++;
    }
    
    public void ResetLevelNumber()
    {
        _levelNum = defaultLevelNum;
    }

    private void GetLevelNum()
    {
        string levelName = SceneManager.GetActiveScene().name;
        _levelNum =  Convert.ToInt32(levelName.Split('-').Last());
        defaultLevelNum = _levelNum;
    }
    
    private void AssignClips()
    {
        for (int i = 0; i < _chapterAudioClips.Length;i++)
        {
            AudioClip AC = _chapterAudioClips[i];
            AudioSource AS = _chapterAudioSources[i];
            if (AC && AS)
            {
                AS.clip = AC;
            }
        }
    }

    protected override void Register()
    {
        base.Register();
        _gameLoopManager.OnComplete += IncrementLevelNumber;
        _gameLoopManager.OnPreparation += Play;
        _gameLoopManager.OnRestart += ResetLevelNumber;
    }

    protected override void Deregister()
    {
        base.Deregister();
        _gameLoopManager.OnComplete -= IncrementLevelNumber;
        _gameLoopManager.OnPreparation -= Play;
        _gameLoopManager.OnRestart -= ResetLevelNumber;
    }
}