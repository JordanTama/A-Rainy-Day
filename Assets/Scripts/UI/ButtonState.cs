using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonState : MonoBehaviour
{
    private GameLoopManager _gameLoopManager;
    private Button _myButton;
    public Image playImage;
    public Image stopImage;
    private Material _playMat;
    private Material _stopMat;
    public Material disabledMat;
    [SerializeField] private bool isPreparationState;
    private MainUIController _mainUiController;
    
    private void Awake()
    {
        _mainUiController = GetComponentInParent<MainUIController>();
    }

    private void Start()
    {
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        _myButton = GetComponent<Button>();
        _playMat = playImage.material;
        _stopMat = stopImage.material;
        _gameLoopManager.OnPreparation += PreparationState;
        _gameLoopManager.OnExecution += ExecutionState;
        _gameLoopManager.OnRestart += EnableInteraction;
        _gameLoopManager.OnComplete += DisableInteraction;
        EnableInteraction();
        PreparationState();
    }

    public void OnClick()
    {
        if (isPreparationState)
        {
            _gameLoopManager.Execute();
        }
        else
        {
            _gameLoopManager.SoftResetLevel();
        }
        _mainUiController.PlayMenuButtonAudio();
    }
    

    private void PreparationState()
    {
        isPreparationState = true;
        SetButtonVisuals();
    }
    
    private void ExecutionState()
    {
        isPreparationState = false;
        SetButtonVisuals();
    }

    private void SetButtonVisuals()
    {
        playImage.enabled = isPreparationState;
        stopImage.enabled = !isPreparationState;
        // _myButton.targetGraphic = isPreparationState ? playImage : stopImage;
    }
    
    private void EnableInteraction()
    {
        _myButton.interactable = true;
        playImage.material = _playMat;
        stopImage.material = _stopMat;
    }
    private void DisableInteraction()
    {
        _myButton.interactable = false;
        playImage.material = disabledMat;
        stopImage.material = disabledMat;
    }

    private void OnDestroy()
    {
        _gameLoopManager.OnPreparation -= PreparationState;
        _gameLoopManager.OnExecution -= ExecutionState;
        _gameLoopManager.OnRestart -= EnableInteraction;
        _gameLoopManager.OnComplete -= DisableInteraction;
    }
}
