using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSetterController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioMixer m = ServiceLocator.Current.Get<AudioManager>().Mixer;
        SaveData Data = ServiceLocator.Current.Get<SettingsManager>().Data;

        m.SetFloat("MasterVolume", 20 * Mathf.Log10(Data.MasterVolume));
        m.SetFloat("MusicVolume", 20 * Mathf.Log10(Data.MusicVolume));
        m.SetFloat("AmbientVolume", 20 * Mathf.Log10(Data.AmbientVolume));
        m.SetFloat("SFXVolume", 20 * Mathf.Log10(Data.SoundEffectsVolume));

        Destroy(gameObject);
    }
}
