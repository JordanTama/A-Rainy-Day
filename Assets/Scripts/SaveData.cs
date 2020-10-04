using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int UpToLevel = 0;
    public Vector2 Resolution;

    public float MasterVolume = 1;
    public float AmbientVolume = 1;
    public float SoundEffectsVolume = 1;
    public float MusicVolume = 1;

    public bool Chapter2Unlocked = false;
    public bool Chapter3Unlocked = false;
}
