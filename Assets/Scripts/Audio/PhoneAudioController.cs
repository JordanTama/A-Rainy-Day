using UnityEngine;

public class PhoneAudioController : AudioController
{
    private TextMessageManager _textMessageManager;

    [SerializeField] private AudioSource lightAudioSource;
    [SerializeField] private AudioClip lightAudioClip;
    
    [SerializeField] private AudioSource showPhoneAudioSource;
    [SerializeField] private AudioClip phoneShowAudioClip;
    [SerializeField] private AudioClip phoneHideAudioClip;
    
    protected override void Awake()
    {
        base.Awake();
        if (lightAudioClip && lightAudioSource)
        {
            lightAudioSource.clip = lightAudioClip;
        }
        _textMessageManager = ServiceLocator.Current.Get<TextMessageManager>();
    }

    protected override void Start()
    {
        base.Start();
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource) _audioSource.clip = _currentAudioClip;

    }

    private void PlayMessageAudio(TextMessage message)
    {
        Play();
    }
    
    private void PlayLightAudio()
    {
        if (!lightAudioSource) return;
        if (lightAudioSource.isPlaying) lightAudioSource.Stop();
        lightAudioSource.Play();
    }
    
    private void PlayShowingAudio(bool isShowing)
    {
        if (!showPhoneAudioSource) return;
        if (lightAudioSource.isPlaying) showPhoneAudioSource.Stop();
        showPhoneAudioSource.clip = isShowing ? phoneHideAudioClip : phoneShowAudioClip; 
        showPhoneAudioSource.Play();
    }

    
    public override void Play()
    {
        base.Play();
        if (!_audioSource) return;
        if (_audioSource.isPlaying) _audioSource.Stop();
        _audioSource.Play();
    }

    public override void Stop()
    {
        base.Stop();
        if (_audioSource)
        {
            _audioSource.Stop();
            lightAudioSource.Stop();
            showPhoneAudioSource.Stop();
        }
    }

    protected override void Register()
    {
        base.Register();
        //_textMessageManager.OnNewTextMessage += PlayMessageAudio;
        //_textMessageManager.OnLightFlash += PlayLightAudio;
        //_textMessageManager.OnPhoneShow += PlayShowingAudio;
        _gameLoopManager.OnPreparation += Stop;
    }

    
    protected override void Deregister()
    {
        base.Deregister();
        //_textMessageManager.OnNewTextMessage -= PlayMessageAudio;
        //_textMessageManager.OnLightFlash -= PlayLightAudio;
        //_textMessageManager.OnPhoneShow -= PlayShowingAudio;
        _gameLoopManager.OnPreparation -= Stop;
    }
}
