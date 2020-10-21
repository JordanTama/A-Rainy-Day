using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    private Slider _slider;
    private AudioSource audioSource;
    private SoundtrackController _soundtrackController;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.value = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        _soundtrackController = FindObjectOfType<SoundtrackController>();
        if (_soundtrackController) audioSource = _soundtrackController._chapterAudioSources[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource)
        {
            if (audioSource.clip)
            {
                _slider.value = audioSource.time / audioSource.clip.length;
            }
        }
    }
}
