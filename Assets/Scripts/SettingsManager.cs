using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : IGameService
{
    public SaveData Data { get; private set; }
    public Action OnApplySettings;

    public int[] BuildIndexExceptions = new int[] { 0 };

    private AudioManager _audioManager;

    public SettingsManager()
    {
        Data = LoadSettings();
        SceneManager.activeSceneChanged += OnLevelComplete;

        new GameObject("Audio Setter", typeof(AudioSetterController));

        OnApplySettings += ApplySettings;
        ApplySettings();
    }

    void ApplySettings()
    {
        if (Data.ResolutionX != 0 && Data.ResolutionY != 0)
            Screen.SetResolution(Data.ResolutionX, Data.ResolutionY, Data.FullScreen, Data.RefreshRate);
        else
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, Data.FullScreen, Screen.currentResolution.refreshRate);
    }

    private void OnLevelComplete(Scene arg0, Scene arg1)
    {
        if (arg1.buildIndex > Data.UpToLevel && !BuildIndexExceptions.Contains(arg1.buildIndex))
        {
            Data.UpToLevel = arg1.buildIndex;

            if (arg1.name.EndsWith("2-1"))
            {
                Data.Chapter2Unlocked = true;
            }

            if (arg1.name.EndsWith("3-1"))
            {
                Data.Chapter3Unlocked = true;
            }

            SaveSettings();
        }
    }

    public SaveData LoadSettings()
    {
        if(File.Exists($"{Application.dataPath}/settings.json"))
            return JsonUtility.FromJson<SaveData>(LoadJsonFromFile());

        return new SaveData();
    }

    public void SaveSettings()
    {
        Debug.Log(Data.UpToLevel);
        OnApplySettings?.Invoke();
        File.WriteAllText($"{Application.dataPath}/settings.json", JsonUtility.ToJson(Data, true));
    }

    public string LoadJsonFromFile()
    {
        if(File.Exists($"{Application.dataPath}/settings.json"))
            return File.ReadAllText($"{Application.dataPath}/settings.json");

        return "";
    }
}
