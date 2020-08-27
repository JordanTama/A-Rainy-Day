using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using DG.Tweening;

public class FeedbackController : MonoBehaviour
{
    [SerializeField] private TMP_InputField feedbackText;
    [SerializeField] private RectTransform feedbackRect;

    private bool isOpen = false;

    public void ToggleFeedback()
    {
        isOpen = !isOpen;
        if(!isOpen)
            feedbackRect.DOAnchorPosY(0, 0.25f);
        else
            feedbackRect.DOAnchorPosY(-356, 0.25f);
    }

    [ContextMenu("Send Feedback")]
    public void SendFeedback()
    {
        StartCoroutine("SendForm");
    }

    IEnumerator SendForm()
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1886026233", feedbackText.text);

        using (var w = UnityWebRequest.Post("https://docs.google.com/forms/u/1/d/e/1FAIpQLScdY6tqH9NTJMjoYoCIPWkUb3icXs78g5k-SSLsNoSUfF4lfA/formResponse", form))
        {
            yield return w.SendWebRequest();
            if(w.isNetworkError || w.isHttpError)
            {
                Debug.LogError(w.error);
            }
            else
            {
                Debug.Log("Feedback sent.");
            }
        }
    }
}
