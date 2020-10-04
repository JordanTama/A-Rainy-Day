using System;
using DG.Tweening;
using UnityEngine;


public abstract class InteractableReceiver : MonoBehaviour
{
    public InteractableController interactableController;

    [SerializeField] protected float lerpTime =1f;
    [SerializeField] protected Ease easeType = Ease.InOutCubic;

    protected AudioSource myAudioSource;
    public float audioVolume=1f;
    
    [SerializeField] protected float fadePercentage = 0.75f;
    protected bool isFading = false;
    
    
    protected void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
        if (myAudioSource) myAudioSource.volume = audioVolume;
    }

    protected void Start()
    {
        if (interactableController)
        {
            interactableController.OnInteractableStateChange += ChangeState;
            interactableController.OnInteractableReset += ResetState;
            
        }
    }

    protected virtual void ChangeState()
    {
        
    }

    protected virtual void ResetState()
    {
        StopAudio();
    }

    protected void PlayAudioWithClip(AudioClip clip)
    {
        if (!myAudioSource) return;
        if (!clip) return;
        myAudioSource.clip = clip;
        PlayAudio();
    }

    protected void PlayAudio()
    {
        if (!myAudioSource) return;
        if (!myAudioSource.clip) return;
        if(myAudioSource.isPlaying) myAudioSource.Stop();
        myAudioSource.Play();
    }

    protected void StopAudio()
    {
        if (!myAudioSource) return;
        if (!myAudioSource.clip) return;
        if (myAudioSource.isPlaying)
        {
            myAudioSource.DOFade(0f, 0f).OnComplete(ResetAudio);
        }
    }
    
    protected void FadeOutAudio(float fadeTime)
    {
        print("Fade out occurred");
        if (!myAudioSource) return;
        if (!myAudioSource.clip) return;
        if (myAudioSource.isPlaying)
        {
            isFading = true;
            myAudioSource.DOFade(0f, fadeTime).SetEase(easeType).OnComplete(ResetAudio);
        }
    }
    
    protected void ResetAudio()
    {
        isFading = false;
        myAudioSource.Stop();
        myAudioSource.volume = audioVolume;
    }

    protected void OnDestroy()
    {
        if (interactableController)
        {
            interactableController.OnInteractableStateChange -= ChangeState;
            interactableController.OnInteractableReset -= ResetState;
        }
        
    }
}
