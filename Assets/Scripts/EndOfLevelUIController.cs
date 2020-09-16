using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class EndOfLevelUIController : MonoBehaviour
{
    private GameLoopManager _loopManager;

    [SerializeField] private string formInputID;
    [SerializeField] private TMP_InputField feedbackText;
    [SerializeField] private TextMeshProUGUI feedbackBtnText;
    [SerializeField] private RectTransform feedbackRect;
    [SerializeField] private Button feedbackBtn;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI responseText;

    // Start is called before the first frame update
    void Start()
    {
        _loopManager = ServiceLocator.Current.Get<GameLoopManager>();
        _loopManager.OnComplete += ShowUI;
        _loopManager.OnRestart += HideUI;
    }

    [ContextMenu("Show")]
    private void ShowUI()
    {
        canvasGroup.DOFade(1, 1);
        canvasGroup.blocksRaycasts = true;
    }
    
    private void HideUI()
    {
        canvasGroup.DOFade(0, 1);
        canvasGroup.blocksRaycasts = false;
    }

    public void SendFeedback()
    {
        feedbackBtnText.text = "...";
        feedbackBtn.interactable = false;
        StartCoroutine("SendForm");
    }

    public void NextLevel(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator SendForm()
    {
        WWWForm form = new WWWForm();
        form.AddField(formInputID, feedbackText.text);

        using (var w = UnityWebRequest.Post("https://docs.google.com/forms/d/e/1FAIpQLScdY6tqH9NTJMjoYoCIPWkUb3icXs78g5k-SSLsNoSUfF4lfA/formResponse", form))
        {
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError)
            {
                Debug.LogError(w.error);
                feedbackBtnText.text = "Something went wrong. :(";
            }
            else
            {
                Debug.Log("Feedback sent.");
                feedbackBtnText.text = "Thank you! :)";
            }
        }
    }

    private void OnDestroy()
    {
        _loopManager.OnComplete -= ShowUI;
    }
}
