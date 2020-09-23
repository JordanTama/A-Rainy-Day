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
            if(isExtended) RetractRamp(1);
            else ExtendRamp(1);
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
        _extendTween = _visualsTransform.DOLocalMove(_extendPosition,t).OnComplete(MeshUpdate);
    }

    private void RetractRamp(float t)
    {
        isExtended = false;
        _extendTween = _visualsTransform.DOLocalMove(_retractPosition,t).OnComplete(MeshUpdate);
    }
    
    private void MeshUpdate()
    {
        _tileManager.OnUpdateMesh?.Invoke();
        isTweening = false;
    }
}
