using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    private Scene oldScene;
    private Scene transitionScene;
    private Scene nextScene;
    private int nextSceneIndex;

    private GameLoopManager _gameLoopManager;
    private Camera mainCamera;

    public Transform uiHideFogTransform;
    public AudioClip hideAudioClip;
    public AudioClip showAudioClip;
    private AudioSource _audioSource;
    
    public bool isNextSceneCinematic = false;


    // Start is called before the first frame update

    private void Awake()
    {
        mainCamera = Camera.main;
        _audioSource = GetComponent<AudioSource>();
        
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        _gameLoopManager.OnComplete += LoadTransitionScene;
        
        oldScene = SceneManager.GetActiveScene();
        transitionScene = SceneManager.GetSceneByName("TransitionScene");
        nextSceneIndex = oldScene.buildIndex + 1;

    }

    void Start()
    {
        SceneManager.sceneLoaded += TransitionToNewScene;
        SceneManager.sceneUnloaded += LoadNewScene;
    }

    private void LoadTransitionScene()
    {
        SceneManager.LoadSceneAsync("TransitionScene", LoadSceneMode.Additive);
    }

    private void UnloadOldScene()
    {
        SceneManager.SetActiveScene(transitionScene);
        SceneManager.UnloadSceneAsync(oldScene, UnloadSceneOptions.None);
    }

    private void LoadNewScene(Scene unloadedScene)
    {
        if (unloadedScene == transitionScene) return;
        
        if (!isNextSceneCinematic)
        {
            SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadSceneAsync("CinematicScene", LoadSceneMode.Additive);
        }
    }

    private void TransitionToNewScene(Scene loadedScene, LoadSceneMode loadMode)
    {
        if (loadedScene.name.Equals("TransitionScene"))
        {
            transitionScene = loadedScene;
            SceneManager.MoveGameObjectToScene(gameObject,transitionScene);
            SceneManager.MoveGameObjectToScene(mainCamera.gameObject,transitionScene);
            HideWithFog();
            // UnloadOldScene();
        }
        else
        {
            nextScene = loadedScene;
            mainCamera.enabled = false;
            mainCamera.GetComponent<AudioListener>().enabled = false;
            RevealWithFog();
        }
    }

    private void HideWithFog()
    {
        PlayAudio(hideAudioClip);
        
        transform.DOMoveY(10f, 2f).OnStart(() =>
        {
            _audioSource.DOFade(0f, 5f).SetEase(Ease.OutCubic);
            uiHideFogTransform.DOLocalMoveY(-0.4f, 1f).SetDelay(1f).OnComplete(UnloadOldScene).SetEase(Ease.InOutCubic);
        }).SetEase(Ease.InOutCubic);
    }

    private void RevealWithFog()
    {
        PlayAudio(showAudioClip);

        uiHideFogTransform.DOLocalMoveY(-1f, 1f).OnStart(() =>
        {
            
            transform.DOMoveY(-15f, 3f).SetDelay(0.5f).OnComplete(() =>
            {
                SceneManager.SetActiveScene(nextScene);
                UnloadTransition();
            }).SetEase(Ease.InOutCubic);
        }).SetEase(Ease.InOutCubic);


    }

    private void UnloadTransition()
    {
        SceneManager.UnloadSceneAsync(transitionScene);
    }

    private void PlayAudio(AudioClip clip)
    {
        if (clip)
        {
            _audioSource.PlayOneShot(clip);
        }
    }

    private void OnDestroy()
    {
        _gameLoopManager.OnComplete -= LoadTransitionScene;
        SceneManager.sceneLoaded -= TransitionToNewScene;
        SceneManager.sceneUnloaded -= LoadNewScene;
    }
}
