using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using DG.Tweening;
using UnityEngine.UI;

public class FeedbackController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI feedbackOpenBtnText;
    [SerializeField] private TMP_InputField feedbackText;
    [SerializeField] private RectTransform feedbackRect;
    [SerializeField] private Button sendButton;
    [SerializeField] private CanvasGroup responsePanel;
    [SerializeField] private TextMeshProUGUI responseText;

    private bool isOpen = false;

    public void ToggleFeedback()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            feedbackRect.DOAnchorPosY(0, 0.25f);
            feedbackOpenBtnText.text = "vv Feedback vv";
        }
        else
        {
            Close();
        }
    }

    [ContextMenu("Send Feedback")]
    public void SendFeedback()
    {
        StartCoroutine("SendForm");
        sendButton.interactable = false;
        responsePanel.DOFade(1, 0.5f);
        responsePanel.blocksRaycasts = true;
        responseText.text = "Sending...";
    }

    public void Close()
    {
        feedbackRect.DOAnchorPosY(-356, 0.25f);
        responsePanel.blocksRaycasts = false;
        responsePanel.alpha = 0;
        sendButton.interactable = true;
        feedbackText.text = "";
        isOpen = false;
        feedbackOpenBtnText.text = "^^ Feedback ^^";
    }
}
