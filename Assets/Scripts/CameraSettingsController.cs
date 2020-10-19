using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraSettingsController : MonoBehaviour
{
    private SettingsManager sm;

    [SerializeField] private PostProcessProfile _postProcessProfile;
    [SerializeField] private PostProcessLayer _postProcessLayer;
    [SerializeField] private Aura2API.AuraCamera _auraCamera;

    private void Awake()
    {
        sm = ServiceLocator.Current.Get<SettingsManager>();

        AmbientOcclusion ssao;
        _postProcessProfile.TryGetSettings(out ssao);
        ssao.active = sm.Data.AmbientOcclusion;

        _postProcessLayer.antialiasingMode = sm.Data.AntiAliasing ? PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing : PostProcessLayer.Antialiasing.None;
        _auraCamera.enabled = sm.Data.VolumetricFog;
        sm.OnApplySettings += ApplySettings;
    }

    void ApplySettings()
    {
        AmbientOcclusion ssao;
        _postProcessProfile.TryGetSettings(out ssao);
        ssao.active = sm.Data.AmbientOcclusion;

        _postProcessLayer.antialiasingMode = sm.Data.AntiAliasing ? PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing : PostProcessLayer.Antialiasing.None;
        _auraCamera.enabled = sm.Data.VolumetricFog;
    }

    private void OnDestroy()
    {
        sm.OnApplySettings -= ApplySettings;
    }
}
