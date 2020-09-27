using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenuUIController : MonoBehaviour
{

    [SerializeField] private float _tweenSpeed;
    [SerializeField] private RectTransform _mainMenu;
    [SerializeField] private RectTransform _chapterSelect;

    private void Start()
    {
        _chapterSelect.anchoredPosition = new Vector2(1920, 0);
    }

    public void LoadLevel(string LevelName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(LevelName);
    }

    public void FromMainToChapterSelect()
    {
        _mainMenu.DOAnchorPosX(-1920f, _tweenSpeed);
        _chapterSelect.DOAnchorPosX(0f, _tweenSpeed);
    }

    public void FromChapterSelectToMain()
    {
        _chapterSelect.DOAnchorPosX(1920f, _tweenSpeed);
        _mainMenu.DOAnchorPosX(0f, _tweenSpeed);
    }
}
