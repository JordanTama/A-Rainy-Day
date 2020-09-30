using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 levelCenter;
    [SerializeField] private float rotateSpeed;
    
    public bool RightClickDown { set; private get; }
    public Vector2 MouseDelta { set; private get; }

    private CameraManager cameraManager;
    private InputManager input;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        input = ServiceLocator.Current.Get<InputManager>();
        input.P_LeftClick.performed += OnLeftClickDown;
        input.P_MouseDelta.performed += OnMouseMoved;
        input.P_MouseDelta.canceled += OnMouseMoved;
        input.P_RightClick.performed += OnRightClickDown;
        input.P_RightClick.canceled += OnRightClickDown;

        cameraManager = ServiceLocator.Current.Get<CameraManager>();

        cam = GetComponent<Camera>();
    }


    private void OnMouseMoved(InputAction.CallbackContext obj)
    {
        Ray ray = cam.ScreenPointToRay(input.P_MousePosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Hit")))
        {
            cameraManager.worldSpaceMousePos = hit.point;
            cameraManager.worldSpaceMousePos.y = 0;
        }

        MouseDelta = obj.performed ? obj.ReadValue<Vector2>() : Vector2.zero;
    }

    private void OnLeftClickDown(InputAction.CallbackContext obj)
    {
        Ray ray = cam.ScreenPointToRay(input.P_MousePosition.ReadValue<Vector2>());
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, 100);
        
        if(hits.Length > 0)
        {
            cameraManager.FireOnRaycastHit(hits);
        }
    }

    private void LateUpdate()
    {
        RotateCamera();
    }

    private void OnRightClickDown(InputAction.CallbackContext obj)
    {
        RightClickDown = obj.performed ? true : false;

    }

    public void RotateCamera()
    {
        if (RightClickDown)
            transform.RotateAround(levelCenter, Vector3.up, MouseDelta.x * rotateSpeed * Time.smoothDeltaTime);
    }

    private void OnDestroy()
    {
        input.P_LeftClick.performed -= OnLeftClickDown;
        input.P_MouseDelta.performed -= OnMouseMoved;
        input.P_MouseDelta.canceled -= OnMouseMoved;
        input.P_RightClick.performed -= OnRightClickDown;
        input.P_RightClick.canceled -= OnRightClickDown;
    }
}
