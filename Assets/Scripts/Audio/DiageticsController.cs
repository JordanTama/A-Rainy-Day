using DG.Tweening;
using UnityEngine;


public class DiageticsController : AudioController
{

    [SerializeField] private AudioClip[] diageticAudioClips = new AudioClip[3];
    [SerializeField] private AudioSource[] diageticAudioSources = new AudioSource[3];

    private AudioSource rainAS;
    private AudioSource footstepsAS;
    private AudioSource crowdAS;

    [SerializeField] private float rainVolume = 1;
    [SerializeField] private float footstepVolume = 1;
    [SerializeField] private float crowdVolume = 1;

    protected override void Awake()
    {
        base.Awake();
        AssignClips();
        PlayRain();
        

    }



    protected override void Start()
    {
        base.Start();
    }

    private void AssignClips()
    {
        for (int i = 0; i < diageticAudioClips.Length; i++)
        {
            AudioClip AC = diageticAudioClips[i];
            AudioSource AS = diageticAudioSources[i];
            if (AC && AS)
            {
                AS.clip = AC;

                if (i == 0) rainAS = AS;
                if (i == 1) footstepsAS = AS;
                if (i == 2) crowdAS = AS;
            }
        }
    }

    public override void Play()
    {

    }

    private void PlayRain()
    {
        if (rainAS)
        {
            if (rainAS.clip)
            {
                if (!rainAS.isPlaying)
                {
                    rainAS.Play();
                }
            }

            rainAS.volume = rainVolume;
        }
    }

    private void PlayCrowdSounds()
    {
        PlayFootsteps();
        PlayCrowdTalking();
    }

    private void PlayFootsteps()
    {
        if (footstepsAS)
        {
            if (footstepsAS.clip)
            {
                if (!footstepsAS.isPlaying)
                {
                    footstepsAS.Play();
                }
            }

            footstepsAS.DOFade(footstepVolume, 1f);
        }
    }

    private void PlayCrowdTalking()
    {
        if (crowdAS)
        {
            if (crowdAS.clip)
            {
                if (!crowdAS.isPlaying)
                {
                    crowdAS.Play();
                }
            }

            crowdAS.DOFade(crowdVolume,1f);
        }
    }

    private void StopCrowdSounds()
    {
        StopFootsteps();
        StopCrowdTalking();

    }

    private void StopFootsteps()
    {
        if (footstepsAS)
        {

            if (footstepsAS.isPlaying)
            {
                footstepsAS.DOFade(0f,1f);
            }
            
        }
    }

    private void StopCrowdTalking()
    {
        if (crowdAS)
        {

            if (crowdAS.isPlaying)
            {
                crowdAS.DOFade(0f,1f);
            }
            
        }
    }
    
    private void StopRain()
    {
        if (rainAS)
        {
            if (rainAS.isPlaying)
            {
                rainAS.DOFade(0f,1f);
            }
            
        }
    }

    public override void Pause()
    {
        base.Pause();
    }

    public override void Stop()
    {
        base.Stop();
        StopCrowdSounds();
        StopRain();
        

    }

    protected override void Register()
    {
        base.Register();
        _gameLoopManager.OnExecution += PlayCrowdSounds;
        _gameLoopManager.OnPreparation += StopCrowdSounds;
        _gameLoopManager.OnComplete += StopCrowdSounds;
    }

    protected override void Deregister()
    {
        base.Deregister();
        _gameLoopManager.OnExecution -= PlayCrowdSounds;
        _gameLoopManager.OnPreparation -= StopCrowdSounds;
        _gameLoopManager.OnComplete -= StopCrowdSounds;
    }
    
    
}
