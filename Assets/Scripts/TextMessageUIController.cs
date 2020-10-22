using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class TextMessageUIController : MonoBehaviour
{
    [SerializeField] private RectTransform mainBody;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float initHeight = 50.0f;
    [SerializeField] private float tweenSpeed = 1.0f;
    [SerializeField] private float minumumTimeToShow = 1.0f;

    private string textBuffer;
    private bool waiting = false;
    private float transformHeight;
    private float timeToExpand = 0;
    private TextMessageMainUIController controller;

    public void Init(string text)
    {
        messageText.text = text;
        LayoutRebuilder.ForceRebuildLayoutImmediate(mainBody);
        transformHeight = mainBody.rect.height;
        mainBody.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, transformHeight);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, transformHeight);
    }

    public void Init(TextMessageMainUIController uIController, float timeToWait, string text)
    {
        messageText.text = text;
        controller = uIController;
        timeToExpand = Mathf.Max(minumumTimeToShow, timeToWait);
        textBuffer = text;
        waiting = true;
        AfterInit();
    }

    private void Update()
    {
        if (waiting)
        {

        }
    }

    private void AfterInit()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(mainBody);
        if (mainBody.rect.height < 50)
            transformHeight = 50;
        else
            transformHeight = mainBody.rect.height;

        if (mainBody.GetComponent<ContentSizeFitter>())
            mainBody.GetComponent<ContentSizeFitter>().enabled = false;
        mainBody.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, initHeight);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, initHeight);

        messageText.text = "...";
        DOTween.To(() => timeToExpand, x => timeToExpand = x, 1, timeToExpand).OnComplete(() => ExpandMessage());
    }

    private void ExpandMessage()
    {
        float f = initHeight;
        DOTween.To(() => f, x => f = x, transformHeight, tweenSpeed).OnUpdate(() => {
            mainBody.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, f);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, f);
        }).SetEase(Ease.Linear);
        messageText.text = textBuffer;
        controller.SentMessage();
    }
}
