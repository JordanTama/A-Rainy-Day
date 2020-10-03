using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class MovableTruck : InteractableReceiver
{
    public bool test;
    private RaycastHit _hit;
    
    [SerializeField] private Transform transformToMove;
    private Vector3 _defaultLocalPosition;
    private Vector3 _endLocalPosition;
    public float moveForwardDistance;
    private TileManager _tileManager;
    private bool _bisAtEnd;
    private bool _isMoving;
    private Tween _moveTween;
    public LayerMask boxCastLayers;

    [SerializeField] private AudioClip MoveForwardAudioClip;
    [SerializeField] private AudioClip MoveBackwardsAudioClip;

    // default lerp time is 2 sec
    // default ease is InOutCubic
 
    protected new void Awake()
    {
        base.Awake();
        _defaultLocalPosition = transformToMove.localPosition;
        _endLocalPosition = _defaultLocalPosition + (moveForwardDistance-transformToMove.localScale.z)*(transformToMove.localRotation.normalized*Vector3.forward);

    }
    protected new void Start()
    {
        base.Start();
        _tileManager = ServiceLocator.Current.Get<TileManager>();
        ResetState();
    }

    private void MoveToEnd(float t)
    {
        if (IsPathClear())
        {
            _bisAtEnd = true;
            _endLocalPosition = _defaultLocalPosition + (moveForwardDistance-transformToMove.localScale.z) *(transformToMove.localRotation.normalized*Vector3.forward);
            _moveTween = transformToMove.DOLocalMove(_endLocalPosition, t).SetEase(easeType).OnComplete(MeshUpdate);
            
            if (t > 0 && MoveForwardAudioClip)
            {
                _moveTween.OnUpdate(CheckForFade);
                PlayAudioWithClip(MoveForwardAudioClip);
            }
        }
        
    }
    
    private void MoveToStart(float t)
    {
        if (IsPathClear())
        {
            _bisAtEnd = false;
            _moveTween = transformToMove.DOLocalMove(_defaultLocalPosition, t).SetEase(easeType).OnComplete(MeshUpdate);

            if (t > 0 && MoveBackwardsAudioClip)
            {
                _moveTween.OnUpdate(CheckForFade);
                PlayAudioWithClip(MoveBackwardsAudioClip);
            }
        }
        
    }

    private void CheckForFade()
    {
        if (_moveTween.ElapsedPercentage() > fadePercentage && !isFading)
        {
            FadeOutAudio((1f-fadePercentage)*lerpTime);
        }
    }

    private void MeshUpdate()
    {
        _tileManager.OnUpdateMesh?.Invoke();
        _isMoving = false;
    }
    
    private bool IsPathClear()
    {
        float directionFactor = _bisAtEnd ? -1f : 1f;
        Vector3 dir = directionFactor*transformToMove.forward;
        float distance = 2.5f * moveForwardDistance;

        Vector3 boxDims = 0.99f*(transformToMove.lossyScale +
                          (5f * moveForwardDistance - 2f * transformToMove.lossyScale.z) * Vector3.forward);
        Vector3 boxCentre = transformToMove.position + dir * distance;
        
        bool isHit;
        isHit = Physics.CheckBox(boxCentre, boxDims / 2,
            Quaternion.identity, boxCastLayers, QueryTriggerInteraction.Ignore);
        // Debug.Log("Has BoxCast hit something: "+isHit);
        return !isHit;
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

    private void OnDrawGizmos()
    {
        float directionFactor = _bisAtEnd ? -1f : 1f;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transformToMove.position+directionFactor*transformToMove.forward*transformToMove.lossyScale.z/2,  directionFactor*transformToMove.forward * 5*(moveForwardDistance-1.5f*transformToMove.localScale.z));
        Gizmos.DrawWireCube(transformToMove.position + directionFactor*transformToMove.forward * 5f*moveForwardDistance/2f, 0.99f*(transformToMove.lossyScale+(5*moveForwardDistance-2*transformToMove.lossyScale.z)*Vector3.forward));
    }
}