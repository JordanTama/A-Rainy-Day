using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

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
    [SerializeField] protected Texture[] textures;
    private MeshRenderer _renderer;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;
    private float defaultExponent;
    [SerializeField] private float selectedExponent;
    private MaterialPropertyBlock propertyBlock;
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

        _renderer = GetComponent<MeshRenderer>();
        propertyBlock = new MaterialPropertyBlock();
        defaultColor = _renderer.material.GetColor("_Color");
        defaultExponent = _renderer.material.GetFloat("_Exponent");

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
        ResetState();
    }

    private void ResetState()
    {
        ChangeState(defaultState);
        InteractableDeselect();
    }

    protected void ChangeState(int newState)
    {
        currentState = newState;
        SetMaterial();
        SetNextState();
        OnInteractableStateChange?.Invoke();
        interactableManager.OnInteractableStateChange?.Invoke();
    }

    private void SetMaterial()
    {
        Texture texture = null;
        if (currentState < textures.Length)
        {
             texture = textures[currentState];
        }

        if (_renderer)
        {
            if(texture) propertyBlock.SetTexture("_MainTex",texture);
            propertyBlock.SetColor("_Color",selectedColor);
            propertyBlock.SetFloat("_Exponent",selectedExponent);
            _renderer.SetPropertyBlock(propertyBlock);
        }
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
        if (_renderer)
        {
            propertyBlock.SetColor("_Color",defaultColor);
            propertyBlock.SetFloat("_Exponent",defaultExponent);
            _renderer.SetPropertyBlock(propertyBlock);
        } 
    }

    protected void OnDestroy()
    {
        interactableManager.OnInteractableSelect-=InteractableSelect;
        interactableManager.OnInteractableDeselect-=InteractableDeselect;

        _gameLoopManager.OnPreparation -= ResetInteractable;
    }
}
