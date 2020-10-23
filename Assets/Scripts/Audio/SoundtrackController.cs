using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ChapterAudioContainer
{
    public AudioClip[] audioClips;
}

public class SoundtrackController : AudioController
{

    [SerializeField] private ChapterAudioContainer[] _chapterAudioClips = new ChapterAudioContainer[3];
    public AudioSource[] _chapterAudioSources = new AudioSource[6];
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
                audioSource.DOFade(0f, 4f).SetEase(Ease.InOutCubic);
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
                v.DOFade(0f, 4f).SetEase(Ease.InOutCubic);
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
        _levelNum =  int.Parse(gameObject.scene.name.Split('-').Last());
        defaultLevelNum = _levelNum;
    }
    
    private void AssignClips()

    {
        for (int i = 0; i < _chapterAudioClips[GetChapterNum()].audioClips.Length;i++)
        {
            AudioClip AC = _chapterAudioClips[GetChapterNum()].audioClips[i];
            AudioSource AS = _chapterAudioSources[i];
            if (AC && AS)
            {
                AS.clip = AC;
            }
        }
    }

    int GetChapterNum()
    {
        return int.Parse(gameObject.scene.name.Split('-').First().Last().ToString()) - 1;
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
