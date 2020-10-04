using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class BuildingRotator : InteractableReceiver
{
    [SerializeField] private Transform transformToRotate;
    public int numRotationPositions;
    private Quaternion _defaultRotation;
    private TileManager _tileManager;
    private bool isRotating;
    private Tween _rotateTween;

    [SerializeField] private AudioClip rotateAudioClip;

    // default lerp time is 1 sec
    // default ease is InOutCubic
    
    protected new void Awake()
    {
        base.Awake();
        _defaultRotation = transformToRotate.rotation;
        
        
    }
    protected new void Start()
    {
        base.Start();
        _tileManager = ServiceLocator.Current.Get<TileManager>();
        ResetState();
    }

    private void Rotate(float t)
    {
        if (numRotationPositions == 0)
        {
            numRotationPositions = 1;
        }
        Quaternion newRotation = Quaternion.AngleAxis(360.0f/numRotationPositions,Vector3.up);
        Quaternion endRotation = transformToRotate.rotation * newRotation;
        if (!isRotating)
        {
            isRotating = true;
            _rotateTween = transformToRotate.DORotate(endRotation.eulerAngles, t).SetEase(easeType).OnComplete(MeshUpdate);
            if (t > 0 && rotateAudioClip)
            {
                PlayAudioWithClip(rotateAudioClip);
            }
        }
    }

    private void MeshUpdate()
    {
        _tileManager.OnUpdateMesh?.Invoke();
        isRotating = false;
    }

    protected override void ChangeState()
    {
        base.ChangeState();
        Rotate(lerpTime);
    }

    protected override void ResetState()
    {
        base.ResetState();
        _rotateTween?.Complete();
        transformToRotate.rotation = _defaultRotation;
    }
}
