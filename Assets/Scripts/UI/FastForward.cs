using UnityEngine;
using UnityEngine.UI;

public class FastForward : MonoBehaviour
{
    private GameLoopManager _gameLoopManager;
    private Button _myButton;
    public Material FFMat;
    public Material NonFFMat;
    public Image FFImage;
    public AIManager AiManager;
    public float FFMultiplier;
    private float _defaultSpeed;
    [SerializeField] private bool isFF;
    
    private void Start()
    {
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();
        _myButton = GetComponent<Button>();
        _gameLoopManager.OnPreparation += MakeNormal;
        _gameLoopManager.OnRestart += EnableInteraction;
        _gameLoopManager.OnComplete += DisableInteraction;
        _gameLoopManager.OnComplete += MakeNormal;
        if (AiManager) _defaultSpeed = AiManager.defaultSimulationSpeed;
        EnableInteraction();
        MakeNormal();
    }
    
    public void OnClick()
    {
        if (isFF)
        {
            MakeNormal();
        }
        else
        {
            MakeFF();
        }
    }
    

    private void MakeNormal()
    {
        AiManager.simulationSpeed = _defaultSpeed;
        if (AiManager.Speed > 0f)
        {
            AiManager.Play();
        }
        isFF = false;
        FFImage.material = NonFFMat;
    }
    
    private void MakeFF()
    {
        AiManager.simulationSpeed =  FFMultiplier*_defaultSpeed;
        if (AiManager.Speed > 0f)
        {
            AiManager.Play();
        }
        isFF = true;
        FFImage.material = FFMat;
    }
    
    private void EnableInteraction()
    {
        _myButton.interactable = true;
    }
    private void DisableInteraction()
    {
        _myButton.interactable = false;
    }

    private void OnDestroy()
    {
        _gameLoopManager.OnPreparation -= MakeNormal;
        _gameLoopManager.OnRestart -= EnableInteraction;
        _gameLoopManager.OnComplete -= DisableInteraction;
        _gameLoopManager.OnComplete -= MakeNormal;
    }
}
