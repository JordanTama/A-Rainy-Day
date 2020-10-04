using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class BuildingHeightAdjuster : InteractableReceiver
{
    [SerializeField] private Transform transformToMove;
    private Vector3 _defaultLocalPosition;
    private Vector3 _endLocalPosition;
    public float yAxisChange;
    private TileManager _tileManager;
    private bool _bisAtEnd;
    private bool _isMoving;
    private Tween _moveTween;
    
    [SerializeField] private AudioClip MoveForwardAudioClip;
    [SerializeField] private AudioClip MoveBackwardsAudioClip;

    // default lerp time is 1 sec
    // default ease is InOutCubic
    
    protected new void Awake()
    {
        base.Awake();
        _defaultLocalPosition = transformToMove.localPosition;
        _endLocalPosition = _defaultLocalPosition + Vector3.up * yAxisChange;

    }
    protected new void Start()
    {
        base.Start();
        _tileManager = ServiceLocator.Current.Get<TileManager>();
        ResetState();
    }

    private void MoveToEnd(float t)
    {
        _bisAtEnd = true;
        _moveTween = transformToMove.DOLocalMove(_endLocalPosition, t).SetEase(easeType).OnComplete(MeshUpdate);
        
        if (t > 0 && MoveForwardAudioClip)
        {
            PlayAudioWithClip(MoveForwardAudioClip);
        }
    }
    
    private void MoveToStart(float t)
    {
        _bisAtEnd = false;
        _moveTween = transformToMove.DOLocalMove(_defaultLocalPosition, t).SetEase(easeType).OnComplete(MeshUpdate);
        
        if (t > 0 && MoveBackwardsAudioClip)
        {
            PlayAudioWithClip(MoveBackwardsAudioClip);
        }
    }

    private void MeshUpdate()
    {
        _tileManager.OnUpdateMesh?.Invoke();
        _isMoving = false;
    }

    protected override void ChangeState()
    {
        base.ChangeState();
        if (_isMoving) return;
        _isMoving = true;
        if (_bisAtEnd)
        {
            MoveToStart(lerpTime);
        }
        else
        {
            MoveToEnd(lerpTime); 
        }
    }

    protected override void ResetState()
    {
        base.ResetState();
        _moveTween?.Complete();
        MoveToStart(0);
    }
}