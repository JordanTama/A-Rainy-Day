using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class InteractableController : MonoBehaviour
{
    public Action OnInteractableStateChange;
    public Action OnInteractableReset;

    protected InteractableManager interactableManager;
    protected InputManager inputManager;
    protected CameraManager cameraManager;
    protected GameLoopManager _gameLoopManager;
    protected AudioManager audioManager;

    [SerializeField] protected int numStates;
    [SerializeField] protected int currentState;
    protected int nextState;
    [SerializeField] protected int defaultState;
    private bool isChangeState;

    protected AudioSource _myAudioSource;
    public AudioClip myAudioClip;
    public float audioVolume=1f;

    private void Awake()
    {
        _myAudioSource = GetComponent<AudioSource>();
        if (myAudioClip && _myAudioSource)
        {
            _myAudioSource.volume = audioVolume;
            _myAudioSource.clip = myAudioClip;
        }
    }

    protected void Start()
    {
        interactableManager = ServiceLocator.Current.Get<InteractableManager>();
        inputManager = ServiceLocator.Current.Get<InputManager>();
        cameraManager = ServiceLocator.Current.Get<CameraManager>();
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        audioManager = ServiceLocator.Current.Get<AudioManager>();
        
        interactableManager.OnInteractableSelect+=InteractableSelect;
        interactableManager.OnInteractableDeselect+=InteractableDeselect;

        _gameLoopManager.OnPreparation += ResetInteractable;

        audioManager.OnAudioStop += StopAudio;

        ResetInteractable();
    }

    protected virtual void ResetInteractable()
    {
        OnInteractableReset?.Invoke();
        StopAudio();
    }

    protected void ChangeState(int newState)
    {
        currentState = newState;
        SetNextState();
        OnInteractableStateChange?.Invoke();
        interactableManager.OnInteractableStateChange?.Invoke();
    }

    protected void SetNextState()
    {
        nextState = (currentState + 1) % numStates;
    }

    protected void InteractableSelect(GameObject obj)
    {
        if (obj == gameObject)
        {
            ChangeState(nextState);
            PlayAudio();
        }
    }

    protected void PlayAudio()
    {
        if (!_myAudioSource) return;
        if (!_myAudioSource.clip) return;
        if(_myAudioSource.isPlaying) _myAudioSource.Stop();
        _myAudioSource.Play();
    }

    protected void StopAudio()
    {
        if (!_myAudioSource) return;
        if (!_myAudioSource.clip) return;
        if (_myAudioSource.isPlaying)
        {
            _myAudioSource.DOFade(0f, 1f).OnComplete(ResetAudio);
        }
    }

    protected void ResetAudio()
    {
        _myAudioSource.Stop();
        _myAudioSource.volume = audioVolume;
    }

    protected void InteractableDeselect()
    {
        // Cancel highlighting
    }

    protected void OnDestroy()
    {
        interactableManager.OnInteractableSelect-=InteractableSelect;
        interactableManager.OnInteractableDeselect-=InteractableDeselect;

        _gameLoopManager.OnPreparation -= ResetInteractable;
    }
}
