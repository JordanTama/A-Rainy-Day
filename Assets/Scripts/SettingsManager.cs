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

    public int[] BuildIndexExceptions = new int[] { 0 };

    public SettingsManager()
    {
        Data = LoadSettings();
        Debug.Log(Data.UpToLevel);
        SceneManager.activeSceneChanged += OnLevelComplete;
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
                Data.Chapter2Unlocked = true;
            }

            SaveSettings();
        }
    }

    public SaveData LoadSettings()
    {
        return JsonUtility.FromJson<SaveData>(LoadJsonFromFile());
    }

    public void SaveSettings()
    {
        Debug.Log(Data.UpToLevel);
        File.WriteAllText($"{Application.dataPath}/settings.json", JsonUtility.ToJson(Data, true));
    }

    public string LoadJsonFromFile()
    {
        if(File.Exists($"{Application.dataPath}/settings.json"))
            return File.ReadAllText($"{Application.dataPath}/settings.json");

        return "";
    }
}
