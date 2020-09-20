using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : IGameService
{
    public Action<RaycastHit[]> OnCameraRaycastHit;
    public Action<int> OnCameraToRotate;
    public Vector3 worldSpaceMousePos;

    private InputManager inputManager;


    public CameraManager(InputManager input)
    {
        inputManager = input;
    }


    public void FireOnRaycastHit(RaycastHit[] hits)
    {
        OnCameraRaycastHit?.Invoke(hits);
    }

    public void RotateCamera(int dir)
    {
        OnCameraToRotate?.Invoke(dir);
    }
}
