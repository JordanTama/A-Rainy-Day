using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleSettingsButton : MonoBehaviour
{

    private GameLoopManager _gameLoopManager;
    private RectTransform _rect;
    [SerializeField] private float defaultXPos;
    [SerializeField] private float hideXPos;

    public float lerpTime = 2f;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        _gameLoopManager.OnComplete += HideControlBar;
        defaultXPos = _rect.anchoredPosition.x;
        hideXPos = defaultXPos + 100f;
        _rect.anchoredPosition += new Vector2(hideXPos,0f);
        
        print(_rect.anchoredPosition);
        
        SceneManager.activeSceneChanged += ShowControlBar;
    }

    private void Start()
    {
        
    }

    private void ShowControlBar(Scene prev, Scene newScene)
    {
        if (newScene.name.Equals("TransitionScene")) return;

        _rect.DOAnchorPosX(defaultXPos, lerpTime).SetEase(Ease.OutCubic);

    }
    
    private void HideControlBar()
    {
        _rect.DOAnchorPosX(hideXPos, lerpTime).SetEase(Ease.InCubic);

    }
    
}