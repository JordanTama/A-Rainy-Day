using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class ExtendableRamp : InteractableReceiver
{
    private Vector3 _retractPosition;
    private Vector3 _extendPosition;
    private Transform _visualsTransform;
    private bool isExtended;
    private bool isTweening;
    public bool bStartExtended;
    private Tween _extendTween;
    private TileManager _tileManager;

    public AudioClip extendAudioClip;
    public AudioClip retractAudioClip;

    // default lerp time is 1 sec
    // default ease is InOutCubic
    
    protected new void Awake()
    {
        base.Awake();
        
        InitializePositions();
        
    }
    protected new void Start()
    {
        base.Start();
        _tileManager = ServiceLocator.Current.Get<TileManager>();
        ResetState();

    }

    protected override void ChangeState()
    {
        base.ChangeState();
        if (!isTweening)
        {
            isTweening = true;
            if(isExtended) RetractRamp(lerpTime);
            else ExtendRamp(lerpTime);
        }
        
    }

    private void InitializePositions()
    {
        _visualsTransform = transform;
        if (bStartExtended)
        {
            
            _extendPosition = _visualsTransform.localPosition;
            _retractPosition = _visualsTransform.localPosition - _visualsTransform.localScale.z * Vector3.forward;
        }
        else
        {
            _extendPosition = _visualsTransform.localPosition + _visualsTransform.localScale.z * Vector3.forward;
            _retractPosition = _visualsTransform.localPosition;
        }
    }

    protected override void ResetState()
    {
        base.ResetState();
        _extendTween?.Complete();
        if(bStartExtended) ExtendRamp(0);
        else RetractRamp(0);
    }

    private void ExtendRamp(float t)
    {
        isExtended = true;
        _extendTween = _visualsTransform.DOLocalMove(_extendPosition,t).SetEase(easeType).OnComplete(MeshUpdate);
        if(t>0f && extendAudioClip) PlayAudioWithClip(extendAudioClip);
    }

    private void RetractRamp(float t)
    {
        isExtended = false;
        _extendTween = _visualsTransform.DOLocalMove(_retractPosition,t).SetEase(easeType).OnComplete(MeshUpdate);
        if(t>0f && retractAudioClip) PlayAudioWithClip(retractAudioClip);
    }
    
    private void MeshUpdate()
    {
        _tileManager.OnUpdateMesh?.Invoke();
        isTweening = false;
    }
}
