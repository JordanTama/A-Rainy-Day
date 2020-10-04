using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SettingsManager : IGameService
{
    public SettingsData Data { get; private set; }

    public SettingsManager()
    {
        Data = LoadSettings();
        Debug.Log(Data.MasterVolume);
    }

    public SettingsData LoadSettings()
    {
        return JsonUtility.FromJson<SettingsData>(LoadJsonFromFile());
    }

    public void SaveSettings()
    {         
        File.WriteAllText($"{Application.dataPath}/settings.json", JsonUtility.ToJson(Data, true));
    }

    public string LoadJsonFromFile()
    {
        if(File.Exists($"{Application.dataPath}/settings.json"))
            return File.ReadAllText($"{Application.dataPath}/settings.json");

        return "";
    }
}
