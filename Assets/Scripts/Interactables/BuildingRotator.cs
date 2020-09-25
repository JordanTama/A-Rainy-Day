using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class BuildingRotator : InteractableReceiver
{
    [SerializeField] private Transform transformToRotate;
    private Quaternion _defaultRotation;
    private TileManager _tileManager;
    private bool isRotating;
    private Tween _rotateTween;
 
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
        Quaternion newRotation = Quaternion.AngleAxis(90,Vector3.up);
        Quaternion endRotation = transformToRotate.rotation * newRotation;
        if (!isRotating)
        {
            isRotating = true;
            _rotateTween = transformToRotate.DORotate(endRotation.eulerAngles, t).OnComplete(MeshUpdate);
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
        Rotate(1);
    }

    protected override void ResetState()
    {
        base.ResetState();
        _rotateTween?.Complete();
        transformToRotate.rotation = _defaultRotation;
    }
}
