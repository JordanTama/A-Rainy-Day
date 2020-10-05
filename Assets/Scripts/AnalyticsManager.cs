using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Net.Http;

public class AnalyticsManager : IGameService
{
    private readonly HttpClient client;

    public AnalyticsManager()
    {
        client = new HttpClient();
        SceneManager.activeSceneChanged += SendSceneAnalytics;
    }

    private void SendSceneAnalytics(Scene arg0, Scene arg1)
    {
        SendFeedback(arg1.name);
    }

    async void SendFeedback(string levelName)
    {
        WWWForm form = new WWWForm();
        var values = new Dictionary<string, string>
        {
            { "entry.798078253", levelName }
        };

        var content = new FormUrlEncodedContent(values);
        var response = await client.PostAsync("https://docs.google.com/forms/u/0/d/e/1FAIpQLScWRvfqHRn2kiSc36CzHeO-2nTAwipdlFESzCzCSX3RFYlPPw/formResponse", content);
    }
}
