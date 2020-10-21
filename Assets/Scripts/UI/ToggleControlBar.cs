using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleControlBar : MonoBehaviour
{

    private GameLoopManager _gameLoopManager;
    private RectTransform _rect;
    [SerializeField] private float defaultYPos;
    [SerializeField] private float hideYPos;

    public float lerpTime = 2f;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        _gameLoopManager.OnComplete += HideControlBar;
        defaultYPos = _rect.anchoredPosition.y;
        hideYPos = defaultYPos - _rect.rect.height;
        _rect.anchoredPosition += new Vector2(0f,hideYPos);
        
        SceneManager.activeSceneChanged += ShowControlBar;
    }

    private void Start()
    {
        
    }

    private void ShowControlBar(Scene prev, Scene newScene)
    {
        if (newScene.name.Equals("TransitionScene")) return;

        _rect.DOAnchorPosY(defaultYPos, lerpTime).SetEase(Ease.OutCubic);

    }
    
    private void HideControlBar()
    {
        _rect.DOAnchorPosY(hideYPos, lerpTime).SetEase(Ease.InCubic);

    }
    
}
