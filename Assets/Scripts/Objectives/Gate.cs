using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gate : MonoBehaviour
{
    [SerializeField] private MeshRenderer mr;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private Animator a;
    
    private static readonly int Active = Animator.StringToHash("Active");
    private static readonly int Ratio = Shader.PropertyToID("_Ratio");


    [ContextMenu("Trigger Gate")]
    public void Trigger()
    {
        if (!a) return;
        
        a.SetBool(Active, true);
    }

    [ContextMenu("Reset Gate")]
    public void Reset()
    {
        if (!a) return;
        
        a.SetBool(Active, false);
    }

    public void SetFill(float ratio, bool burst = true)
    {
        DOTween.To(
            () => mr.material.GetFloat(Ratio),
            (val) => { mr.material.SetFloat(Ratio, val); },
            ratio,
            .5f
            ).OnComplete(() =>
        {
            ps.gameObject.SetActive(ratio >= 1.0f && burst);
        });
    }
}
