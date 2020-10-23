using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int UpToLevel = 0;

    // Video Settings
    public int ResolutionX = 0;
    public int ResolutionY = 0;
    public int RefreshRate = 60;
    public bool FullScreen = true;
    public bool AmbientOcclusion = true;
    public bool VolumetricFog = true;
    public bool AntiAliasing = true;

    // Audio Settings
    public float MasterVolume = 1;
    public float AmbientVolume = 1;
    public float SoundEffectsVolume = 1;
    public float MusicVolume = 1;

    public bool Chapter2Unlocked = true;
    public bool Chapter3Unlocked = true;
}
