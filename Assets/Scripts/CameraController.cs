using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private CameraManager cameraManager;
    private InputManager input;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        input = ServiceLocator.Current.Get<InputManager>();
        input.P_LeftClick.performed += OnLeftClickDown;
        input.P_MouseDelta.performed += OnMouseMoved;

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

    private void OnDestroy()
    {
        input.P_LeftClick.performed -= OnLeftClickDown;
        input.P_MouseDelta.performed -= OnMouseMoved;
    }
}
