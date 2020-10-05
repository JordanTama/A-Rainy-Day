using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UIAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.DOScale(Vector2.one * 1.1f, 0.25f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOScale(Vector2.one, 0.25f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
