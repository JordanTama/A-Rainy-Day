using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class ExtendableRamp : InteractableReceiver
{
    [SerializeField]private Vector3 _retractPosition;
    [SerializeField]private Vector3 _extendPosition;
    [SerializeField] private LayerMask layerMask;
    private Transform _visualsTransform;
    private bool isExtended;
    private bool isTweening;
    public bool bStartExtended;
    public bool bInitialiseAsExtended = true;
    private Tween _extendTween;
    private TileManager _tileManager;

    private NavMeshObstacle obstacle;

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
        obstacle = GetComponentInChildren<NavMeshObstacle>();
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
        if (bInitialiseAsExtended)
        {
            
            _extendPosition =  _visualsTransform.localPosition;
            _retractPosition = _extendPosition - _visualsTransform.localScale.z * Vector3.forward;
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
        _extendTween = _visualsTransform.DOLocalMove(_extendPosition,t).OnStart(()=>isTweening=true).SetEase(easeType).OnComplete(()=>
        {
            if (t > 0f)
            {
                FadeOutAudio(0.1f); 
            }
            
            MeshUpdate();
        });
        if(t>0f && extendAudioClip) PlayAudioWithClip(extendAudioClip);
    }

    private bool CheckForAgent()
    {
        return Physics.CheckBox(transform.position + 1.05f*transform.lossyScale.y * transform.up,
            0.95f*transform.lossyScale/2f,transform.rotation,layerMask);
    }

    private void RetractRamp(float t)
    {
        if (CheckForAgent()) return;

        if (obstacle)
        {
            obstacle.enabled = true;
            obstacle.carving = true;
        }
        
        
        isExtended = false;
        _extendTween = _visualsTransform.DOLocalMove(_retractPosition,t).OnStart(()=>isTweening=true).SetEase(easeType).OnComplete(()=>
        {
            if (t > 0f)
            {
                FadeOutAudio(0.1f); 
                
            }
            
            if (obstacle)
            {
                obstacle.enabled = false;
                obstacle.carving = false;
            }
            
            MeshUpdate();
        });
        if(t>0f && retractAudioClip) PlayAudioWithClip(retractAudioClip);
    }
    
    private void MeshUpdate()
    {
        _tileManager.OnUpdateMesh?.Invoke();
        isTweening = false;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + 1.05f*transform.up*transform.lossyScale.y,
            0.95f*transform.lossyScale);
    }
}
