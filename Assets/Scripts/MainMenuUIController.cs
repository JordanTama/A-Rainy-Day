using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.Linq;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{

    [SerializeField] private float _tweenSpeed;
    [SerializeField] private RectTransform _mainMenu;
    [SerializeField] private RectTransform _chapterSelect;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private Button _chapter2Button;
    [SerializeField] private Button _chapter3Button;
    [SerializeField] private AudioMixer _mixer;

    [Header("Options Menu")] 
    [Header("Audio Settings")]
    [SerializeField] private RectTransform _optionsMenu;
    [SerializeField] private Slider _masterVolume;
    [SerializeField] private Slider _ambientVolume;
    [SerializeField] private Slider _musicVolume;
    [SerializeField] private Slider _sfxVolume;

    [Header("Video Settings")]
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private Toggle _fullscreenToggle;
    [SerializeField] private Toggle _ssaoToggle;
    [SerializeField] private Toggle _volumetricFogToggle;
    [SerializeField] private Toggle _antiAliasingToggle;
    [SerializeField] private PostProcessProfile _postProcessProfile;

    [Header("Jordan, why must you do this to me?")]
    [SerializeField] private AIManager aiManager;

    private void OnEnable()
    {
        ServiceLocator.Current.Get<InputManager>().ToggleInput(false);
        aiManager?.Pause();
    }

    private void OnDisable()
    {
        ServiceLocator.Current.Get<InputManager>().ToggleInput(true);
        if(aiManager?.Speed > 0 || ServiceLocator.Current.Get<GameLoopManager>().gameState == GameLoopManager.GameState.Execution)
            aiManager?.Play();
    }

    private void Start()
    {
        SettingsManager sm = ServiceLocator.Current.Get<SettingsManager>();

        if(_chapterSelect)
            _chapterSelect.anchoredPosition = new Vector2(1920, 0);

        _optionsMenu.anchoredPosition = new Vector2(-1920, 0);

        if (sm.Data.UpToLevel == 0)
        {
            _continueButton.SetActive(false);
        }

        //if (!sm.Data.Chapter2Unlocked && _chapter2Button)
        //{
        //    _chapter2Button.interactable = false;
        //}

        //if (!sm.Data.Chapter3Unlocked && _chapter3Button)
        //{
        //    _chapter3Button.interactable = false;
        //}

        _masterVolume.value = sm.Data.MasterVolume;
        _ambientVolume.value = sm.Data.AmbientVolume;
        _sfxVolume.value = sm.Data.SoundEffectsVolume;
        _musicVolume.value = sm.Data.MusicVolume;

        _resolutionDropdown.ClearOptions();
        var optionData = new List<TMP_Dropdown.OptionData>();
        foreach(Resolution r in Screen.resolutions)
        {
            var data = new TMP_Dropdown.OptionData($"{r.width}x{r.height} @{r.refreshRate}hz");
            optionData.Add(data);
        }
        _resolutionDropdown.AddOptions(optionData);
        _resolutionDropdown.value = GetIndexOfResolution(Screen.currentResolution, Screen.resolutions);

        _fullscreenToggle.isOn = sm.Data.FullScreen;
        _ssaoToggle.isOn = sm.Data.AmbientOcclusion;
        _volumetricFogToggle.isOn = sm.Data.VolumetricFog;
        _antiAliasingToggle.isOn = sm.Data.AntiAliasing;
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

    public void FromMainMenuToOptions()
    {
        _mainMenu.DOAnchorPos3DX(1920, _tweenSpeed);
        _optionsMenu.DOAnchorPosX(0, _tweenSpeed);
    }

    public void FromOptionsToMenu()
    {
        _mainMenu.DOAnchorPos3DX(0, _tweenSpeed);
        _optionsMenu.DOAnchorPosX(-1920, _tweenSpeed);

        RevertOptions();
    }

    public void ContinuePlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ServiceLocator.Current.Get<SettingsManager>().Data.UpToLevel);
    }

    public void RevertOptions()
    {
        AudioMixer m = ServiceLocator.Current.Get<AudioManager>().Mixer;
        SaveData data = ServiceLocator.Current.Get<SettingsManager>().Data;
        m.SetFloat("MasterVolume", 20.0f * Mathf.Log10(data.MasterVolume));
        m.SetFloat("AmbientVolume", 20.0f * Mathf.Log10(data.AmbientVolume));
        m.SetFloat("SFXVolume", 20.0f * Mathf.Log10(data.SoundEffectsVolume));
        m.SetFloat("MusicVolume", 20.0f * Mathf.Log10(data.MusicVolume));

        _masterVolume.value = data.MasterVolume;
        _ambientVolume.value = data.AmbientVolume;
        _sfxVolume.value = data.SoundEffectsVolume;
        _musicVolume.value = data.MusicVolume;
    }

    public void ApplyOptions()
    {
        SaveData data = ServiceLocator.Current.Get<SettingsManager>().Data;
        data.MasterVolume = _masterVolume.value;
        data.AmbientVolume = _ambientVolume.value;
        data.SoundEffectsVolume = _sfxVolume.value;
        data.MusicVolume = _musicVolume.value;

        data.ResolutionX = Screen.resolutions[_resolutionDropdown.value].width;
        data.ResolutionY = Screen.resolutions[_resolutionDropdown.value].height;
        data.AntiAliasing = _antiAliasingToggle.isOn;
        data.VolumetricFog = _volumetricFogToggle.isOn;
        data.FullScreen = _fullscreenToggle.isOn;
        data.AmbientOcclusion = _ssaoToggle.isOn;

        ServiceLocator.Current.Get<SettingsManager>().SaveSettings();
    }

    public void SetMasterVolume(float v)
    {
        ServiceLocator.Current.Get<AudioManager>().Mixer.SetFloat("MasterVolume", 20.0f * Mathf.Log10(v));
    }

    public void SetAmbientVolume(float v)
    {
        ServiceLocator.Current.Get<AudioManager>().Mixer.SetFloat("AmbientVolume", 20.0f * Mathf.Log10(v));
    }

    public void SetSFXVolume(float v)
    {
        ServiceLocator.Current.Get<AudioManager>().Mixer.SetFloat("SFXVolume", 20.0f * Mathf.Log10(v));
    }

    public void SetMusicVolume(float v)
    {
        ServiceLocator.Current.Get<AudioManager>().Mixer.SetFloat("MusicVolume", 20.0f * Mathf.Log10(v));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public static int GetIndexOfResolution(Resolution currentResolution, Resolution[] resolutions)
    {
        if (resolutions.Length == 0)
            return 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].Equals(currentResolution))
                return i;
        }
        return 0;
    }
}
