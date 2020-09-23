using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BuildingRotator : InteractableReceiver
{
    [SerializeField] private Transform transformToRotate;
    private Quaternion _defaultRotation;
    private TileManager _tileManager;
 
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

    private void Rotate()
    {
        transformToRotate.Rotate(Vector3.up,90,Space.Self);
        _tileManager.OnUpdateMesh?.Invoke();
    }

    protected override void ChangeState()
    {
        base.ChangeState();
        Rotate();
    }

    protected override void ResetState()
    {
        base.ResetState();
        transformToRotate.rotation = _defaultRotation;
    }
}
