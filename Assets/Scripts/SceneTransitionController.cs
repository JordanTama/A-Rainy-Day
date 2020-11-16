using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    [SerializeField] private Scene oldScene;
    [SerializeField] private Scene transitionScene;
    [SerializeField] private Scene nextScene;
    [SerializeField] private int nextSceneIndex;

    private GameLoopManager _gameLoopManager;
    private InputManager _inputManager;
    private Camera mainCamera;

    public Transform uiHideFogTransform;
    public AudioClip hideAudioClip;
    public AudioClip showAudioClip;
    private AudioSource _audioSource;

    public Transform tilesTransform;
    public GameObject fogPlane;

    public bool isLowerIntoFog = true;
    
    
    
    public bool isNextSceneCinematic = false;


    // Start is called before the first frame update

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        _gameLoopManager.OnComplete += LoadTransitionScene;
        
        _inputManager = ServiceLocator.Current.Get<InputManager>();
        
        
        transitionScene = SceneManager.GetSceneByName("TransitionScene");

        if(isLowerIntoFog)
        {
            if(!tilesTransform) tilesTransform = GameObject.FindGameObjectWithTag("Pillar").transform.parent;
            tilesTransform.position= new Vector3(0f,-20f,0f);
        }
        
        
    }

    void Start()
    {
        
        
        SceneManager.sceneLoaded += TransitionToNewScene;
        SceneManager.sceneUnloaded += LoadNewScene;
        
        if(isLowerIntoFog) RaiseOutOfFog();
    }

    private void LoadTransitionScene()
    {
        _inputManager.ToggleInput(false);
        oldScene = SceneManager.GetActiveScene();
        nextSceneIndex = oldScene.buildIndex + 1;
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
            mainCamera = Camera.main;
            transitionScene = loadedScene;
            SceneManager.MoveGameObjectToScene(gameObject,transitionScene);
            SceneManager.MoveGameObjectToScene(mainCamera.gameObject,transitionScene);
            if(!isLowerIntoFog) HideWithFog();
            else LowerIntoFog();
        }
        else
        {
            nextScene = loadedScene;
            mainCamera.enabled = false;
            mainCamera.GetComponent<AudioListener>().enabled = false;
            if(!isLowerIntoFog) RevealWithFog();
            else
            {
                SceneManager.SetActiveScene(nextScene);
                
                fogPlane.SetActive(false);
                Invoke("UnloadTransition",0.5f);
            }
        }
        
        print("scene transition happened");
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

    private void LowerIntoFog()
    {
        PlayAudio(hideAudioClip);

        tilesTransform.DOMoveY(-20f, 3f).OnStart(() =>
        {
            _audioSource.DOFade(0f, 5f).SetEase(Ease.InCubic);
        }).OnComplete(UnloadOldScene).SetEase(Ease.InCubic);
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

    private void RaiseOutOfFog()
    {
        _inputManager.ToggleInput(false);
        tilesTransform.DOMoveY(0f, 3f).OnComplete(() =>
        {
            _inputManager.ToggleInput(true);
            _gameLoopManager.OnLevelReady?.Invoke();
            
            PlayAudio(showAudioClip);
        }).SetEase(Ease.OutCubic);
    }

    private void UnloadTransition()
    {
        
        SceneManager.UnloadSceneAsync(transitionScene);
    }

    private void PlayAudio(AudioClip clip)
    {
        if (clip)
        {
            if(_audioSource.isPlaying) _audioSource.Stop();
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
