using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class ExtendableRamp : InteractableReceiver
{
    private Vector3 _retractPosition;
    private Vector3 _extendPosition;
    [SerializeField]private NavMeshObstacle _obstacle;
    [SerializeField] private Transform _visualsTransform;
    private bool isExtended;
    public bool bStartExtended;

    protected new void Awake()
    {
        base.Awake();
        
        InitializePositions();
        
    }
    protected new void Start()
    {
        base.Start();
        ResetState();

    }

    protected override void ChangeState()
    {
        base.ChangeState();
        if(isExtended) RetractRamp();
        else ExtendRamp();
    }

    private void InitializePositions()
    {
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
        if(bStartExtended) ExtendRamp();
        else RetractRamp();
    }

    private void ExtendRamp()
    {
        isExtended = true;
        if (_obstacle)
        {
            _obstacle.carving = !isExtended;
            _obstacle.enabled = !isExtended;
        }
        _visualsTransform.localPosition = _extendPosition;
    }

    private void RetractRamp()
    {
        isExtended = false;
        if (_obstacle)
        {
            _obstacle.carving = !isExtended;
            _obstacle.enabled = !isExtended;
        }
        _visualsTransform.localPosition = _retractPosition;
    }
}
