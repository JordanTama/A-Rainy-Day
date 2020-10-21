using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warp : InteractableReceiver
{
    public bool bStartActivated;
    private bool isPlaySound;
    public Transform endTransform;
    public AIManager AiManager;
    private bool isActive;
    private OffMeshLink _offMeshLink;
    private MeshRenderer _meshRenderer;
    private MeshRenderer _endMeshRenderer;
    private Collider _collider;
    private NavMeshObstacle _obstacle;

    private GameLoopManager _gameLoopManager;
    
    [SerializeField] private AudioSource warpActivationAudioSource;
    [SerializeField] private AudioSource warpUsedAudioSource;
    [SerializeField] private AudioSource warpActiveAudioSource;

    [SerializeField] private AudioClip warpTurnedOnClip;
    [SerializeField] private AudioClip warpTurnedOffClip;
    [SerializeField] private AudioClip warpUsedClip;
    [SerializeField] private AudioClip warpActiveClip;
    
    
    protected new void Awake()
    {
        base.Awake();
        _offMeshLink = GetComponent<OffMeshLink>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _obstacle = GetComponent<NavMeshObstacle>();
        if (endTransform)
        {
            _offMeshLink.endTransform = endTransform;
            _endMeshRenderer = endTransform.GetComponent<MeshRenderer>();
        }

        if (warpActivationAudioSource && warpTurnedOnClip) warpActivationAudioSource.clip = warpTurnedOnClip;
        if (warpUsedAudioSource && warpUsedClip) warpUsedAudioSource.clip = warpUsedClip;
        if (warpActiveAudioSource && warpActiveClip) warpActiveAudioSource.clip = warpActiveClip;
    }

    protected new void Start()
    {
        base.Start();
        
        if (!interactableController)
        {
            _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
            if (_gameLoopManager != null)
            {
                _gameLoopManager.OnPreparation += ResetState;
            }
        }
        
        ResetState();
    }

    protected override void ChangeState()
    {
        
        if (isActive) Deactivate();
        else Activate();
    }

    protected override void ResetState()
    {
        isPlaySound = false;
        if(bStartActivated) Activate();
        else Deactivate();
        
    }

    public void Activate()
    {
        isActive = true;
        SetParametersToState();

    }
    
    public void Deactivate()
    {
        isActive = false;
        SetParametersToState();
    }

    private void SetParametersToState()
    {
        _offMeshLink.activated = isActive;
        _meshRenderer.enabled = isActive;
        _collider.enabled = isActive;
        PlayWarpOnOff(isActive);
        PlayWarpActive();
        // _obstacle.enabled = !isActive;
        if(AiManager) AiManager.UpdateAllAgentsDestination();
        if (_endMeshRenderer) _endMeshRenderer.enabled = isActive;
    }

    private void PlayWarpOnOff(bool isOn)
    {
        if (!warpActivationAudioSource) return;
        if(warpActivationAudioSource.isPlaying)warpActivationAudioSource.Stop();
        warpActivationAudioSource.clip = isOn ? warpTurnedOnClip : warpTurnedOffClip;
        if (isPlaySound)
        {
            warpActivationAudioSource.Play();
        }
        else
        {
            isPlaySound = true;
        }
    }
    
    private void PlayWarpUsed()
    {
        if (!warpUsedAudioSource) return;
        if(warpUsedAudioSource.isPlaying)warpUsedAudioSource.Stop();
        warpUsedAudioSource.Play();
    }
    private void PlayWarpActive()
    {
        if (!warpActiveAudioSource) return;
        if(warpActiveAudioSource.isPlaying)warpUsedAudioSource.Stop();
        warpActiveAudioSource.loop = isActive;
        if(isActive) warpActiveAudioSource.Play();
        else warpActiveAudioSource.Stop();
    }
    
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            AIAgent agent = other.GetComponent<AIAgent>();
            NavMeshAgent navMeshAgent = other.GetComponent<NavMeshAgent>();
            
            if (agent && _offMeshLink)
            {
                navMeshAgent.Warp(_offMeshLink.endTransform.position);
                agent.UpdateDestination();
                PlayWarpUsed();
            }
        }
    }
    
    

    protected new void OnDestroy()
    {
      base.OnDestroy();
      if (_gameLoopManager != null)
      {
          _gameLoopManager.OnPreparation -= ResetState;
      }
    } 
}
