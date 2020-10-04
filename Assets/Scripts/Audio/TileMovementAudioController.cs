using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TileMovementAudioController : AudioController
{
    private TileManager _tileManager;
    public float tileMovementAudioVolume = 0.5f;
    public float pitchShiftPercentage;
    private Tween _volumeTween;
    
    protected override void Awake()
    {
        base.Awake();
        _tileManager = ServiceLocator.Current.Get<TileManager>();
        
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource && _currentAudioClip)
        {
            _audioSource.volume = tileMovementAudioVolume;
            _audioSource.clip = _currentAudioClip;
        }
    }

    public override void Play()
    {
        if (!_audioSource) return;
        if(_audioSource.isPlaying) Stop();
        RandomPitchShift();
        _audioSource.Play();
        _volumeTween = _audioSource.DOFade(0, 0.5f).SetEase(Ease.InExpo);

    }

    private void RandomPitchShift()
    {
        _audioSource.pitch = Random.Range(1f - pitchShiftPercentage / 100f, 1f + pitchShiftPercentage / 100f);
    }

    public override void Stop()
    {
        if (!_audioSource) return;
        _audioSource.Stop();
        _volumeTween?.Complete();
        _audioSource.volume = tileMovementAudioVolume;
        
        
    }

    private void FadeAway()
    {
        if (!_audioSource) return;
        
        _audioSource.DOFade(0f, 0f).OnComplete(() => { Stop(); });
    }

    protected override void Register()
    {
        base.Register();
        _tileManager.OnTileMoving += Play;
        _tileManager.OnNewTilePosition += Stop;
        _gameLoopManager.OnPreparation += Stop;
        
    }

    protected override void Deregister()
    {
        base.Deregister();
        _tileManager.OnTileMoving -= Play;
        _tileManager.OnNewTilePosition -= Stop;
        _gameLoopManager.OnPreparation -= Stop;
    }
}
