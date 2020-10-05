using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
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
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Collider[] agentsToMove;
    [SerializeField] private List<NavMeshAgent> agentsNavMeshList;
    
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
        _moveTween = transformToMove.DOLocalMove(_endLocalPosition, t).SetEase(easeType).OnComplete(()=>
        {
            if (t > 0f)
            {
                FadeOutAudio(0f); 
            }
            MeshUpdate();
        });

        if (t > 0 && MoveForwardAudioClip)
        {
            PlayAudioWithClip(MoveForwardAudioClip);
        }
    }
    
    private void MoveToStart(float t)
    {
        _bisAtEnd = false;
        _moveTween = transformToMove.DOLocalMove(_defaultLocalPosition, t).SetEase(easeType).OnComplete(()=>
        {
            if (t > 0f)
            {
                FadeOutAudio(0f); 
            }
            MeshUpdate();
        });
        
        
        
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

    private bool CheckForAgent()
    {
        // agentsToMove = Physics.OverlapBox(transformToMove.position + 1.01f*transformToMove.lossyScale.y * transformToMove.up,
        //     0.95f*transformToMove.lossyScale/2f,transformToMove.rotation,layerMask);

        return Physics.CheckBox(transformToMove.position + 1.05f*transformToMove.lossyScale.y * transformToMove.up,
            0.95f*transformToMove.lossyScale/2f,transformToMove.rotation,layerMask);
        // foreach (var a in agentsToMove)
        // {
        //     if (a.CompareTag("Player") || a.CompareTag("NPC"))
        //     {
        //         var nav = a.gameObject.GetComponent<NavMeshAgent>();
        //         if (nav)
        //             agentsNavMeshList.Add(nav);
        //     }
        // }
    }

    private void UpdateAgentPositions()
    {
        if (agentsNavMeshList.Count == 0) return;
        foreach (var nav in agentsNavMeshList)
        {
            nav.Warp(new Vector3(nav.transform.position.x,transformToMove.position.y+transformToMove.lossyScale.y+nav.transform.lossyScale.y, nav.transform.position.z));
        }
    }

    protected override void ChangeState()
    {
        base.ChangeState();

        if (CheckForAgent()) return;
        
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
        Gizmos.DrawWireCube(transformToMove.position + 1.05f*transformToMove.lossyScale.y * transformToMove.up,
            0.95f*transformToMove.lossyScale);
    }
    
    
}