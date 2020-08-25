using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileController : MonoBehaviour
{
    private TileManager tileManager;
    private InputManager inputManager;
    private CameraManager cameraManager;
    private Vector3 startMousePos;
    private bool tweening;

    // Start is called before the first frame update
    void Start()
    {
        tileManager = ServiceLocator.Current.Get<TileManager>();
        inputManager = ServiceLocator.Current.Get<InputManager>();
        cameraManager = ServiceLocator.Current.Get<CameraManager>();

        inputManager.P_MouseDelta.performed += MouseMoved;
        tileManager.OnTileSelect += (e) =>
        {
            if(e == gameObject)
                startMousePos = cameraManager.worldSpaceMousePos;
        };
        tileManager.OnTileDeselect += () => startMousePos = Vector3.zero;
    }

    private void MouseMoved(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<Vector2>().magnitude < 0.5f)
            return;


    }

    private void Update()
    {
        if (tileManager.CurrentTile == gameObject)
        {
            if (Vector3.Distance(startMousePos, cameraManager.worldSpaceMousePos) > 2.5f && !tweening)
            {
                var dir = (cameraManager.worldSpaceMousePos.RoundToNearest(5) - transform.position).normalized;
                //newPos += (dir.normalized);
                if (CheckAdjacent(dir))
                {
                    transform.DOMove((transform.position + (dir * 5)).RoundToNearest(5), 0.5f)
                        .OnStart(()=>tweening = true)
                        .OnComplete(()=> {
                        startMousePos = cameraManager.worldSpaceMousePos;
                        tweening = false;
                    });
                }
            }
        }
    }

    bool CheckAdjacent(Vector3 dir)
    {
        Debug.DrawRay(transform.position, dir.normalized * 2.55f);
        if(Physics.BoxCast(transform.position, new Vector3(2f, 0.5f, 2f), dir.normalized, Quaternion.identity, 2.5f))
        {
            return false;
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        
    }
}
