using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextMessageUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float initHeight = 50.0f;
    [SerializeField] private float tweenSpeed = 1.0f;

    public void SetText(string text)
    {
        messageText.text = text;
    }

    private void Start()
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, initHeight);
        float f = initHeight;
        DOTween.To(() => f, x => f = x, 150.0f, tweenSpeed).OnUpdate(() => rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, f));
    }
}
