using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIController : MonoBehaviour
{
    private AudioSource _audioSource;
    public float menuUIButtonVolume = 0.5f;
    [SerializeField] private AudioClip audioClip;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource && audioClip)
        {
            _audioSource.volume = menuUIButtonVolume;
            _audioSource.clip = audioClip;
        }
    }

    public void RotateCamera(int dir)
    {
        ServiceLocator.Current.Get<CameraManager>().RotateCamera(dir);
    }

    public void PlayMenuButtonAudio()
    {
        if (!_audioSource) return;
        if(_audioSource.isPlaying) _audioSource.Stop();
        _audioSource.Play();

    }
}
