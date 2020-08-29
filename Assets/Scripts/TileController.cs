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
    private GameManager _gameManager;
    
    private Vector3 startMousePos;
    private bool tweening;
    
    private Vector3 _startPosition;
    private Quaternion _startRotation;


    // Start is called before the first frame update
    void Start()
    {
        tileManager = ServiceLocator.Current.Get<TileManager>();
        inputManager = ServiceLocator.Current.Get<InputManager>();
        cameraManager = ServiceLocator.Current.Get<CameraManager>();
        _gameManager = ServiceLocator.Current.Get<GameManager>();

        tileManager.OnTileSelect += (e) =>
        {
            if(e == gameObject)
                startMousePos = cameraManager.worldSpaceMousePos;
        };
        tileManager.OnTileDeselect += () => startMousePos = Vector3.zero;

        _gameManager.OnRestart += ResetPosition;
        SetStartPosition();

    }

    private void Update()
    {
        // This can all be moved into some MouseMoved function later on
        if (tileManager.CurrentTile == gameObject)
        {
            if (Vector3.Distance(startMousePos, cameraManager.worldSpaceMousePos) > 2.5f && !tweening)
            {
                var dir = (cameraManager.worldSpaceMousePos.RoundToNearest(5) - transform.position).normalized;
                //newPos += (dir.normalized);
                if (CheckDot(dir))
                {
                    if (CheckAdjacent(dir))
                    {
                        transform.DOMove((transform.position + (dir * 5)).RoundToNearest(5), 0.5f)
                            .OnStart(() => tweening = true)
                            .OnComplete(() =>
                            {
                                startMousePos = cameraManager.worldSpaceMousePos;
                                tweening = false;
                            });
                    }
                }
            }
        }
    }

    bool CheckDot(Vector3 dir)
    {
        if (Mathf.Abs(Vector3.Dot(dir, transform.forward)) > 0.75f)
            return true;

        if (Mathf.Abs(Vector3.Dot(dir, transform.right)) > 0.75f)
            return true;

        return false;
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

    public void SetStartPosition()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }
    
    public void ResetPosition()
    {
        transform.SetPositionAndRotation(_startPosition,_startRotation);
    }
}
