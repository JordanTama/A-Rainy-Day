using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileController : MonoBehaviour
{
    const float TILE_MOVE_SPEED = 0.5f;

    [SerializeField] private bool fixedInPlace;

    private TileManager tileManager;
    private InputManager inputManager;
    private CameraManager cameraManager;
    private GameLoopManager _gameLoopManager;
    
    private Vector3 startMousePos;
    private bool tweening;
    
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Vector2 _tileSize;

    // Start is called before the first frame update
    void Start()
    {
        tileManager = ServiceLocator.Current.Get<TileManager>();
        inputManager = ServiceLocator.Current.Get<InputManager>();
        cameraManager = ServiceLocator.Current.Get<CameraManager>();
        _gameLoopManager = ServiceLocator.Current.Get<GameLoopManager>();

        tileManager.OnTileSelect += TileSelect;
        tileManager.OnTileDeselect += TileDeselect;

        _gameLoopManager.OnRestart += ResetPosition;
        _tileSize = new Vector2(transform.localScale.x, transform.localScale.z);
        SetStartPosition();
    }

    void TileSelect(GameObject g)
    {
        if (g == gameObject)
            startMousePos = cameraManager.worldSpaceMousePos;
    }

    void TileDeselect() => startMousePos = Vector3.zero;

    private void Update()
    {
        if (fixedInPlace)
            return;

        // This can all be moved into some MouseMoved function later on
        if (tileManager.CurrentTile == gameObject)
        {
            if (Vector3.Distance(startMousePos, cameraManager.worldSpaceMousePos) > (_tileSize.x * 0.5f) && !tweening)
            {
                var dir = (cameraManager.worldSpaceMousePos.RoundToNearest(_tileSize.x) - transform.position).normalized;
                //newPos += (dir.normalized);
                if (CheckDot(dir))
                {
                    if (CheckAdjacent(dir))
                    {
                        transform.DOMove((transform.position + (dir * _tileSize.x)).RoundToNearest(_tileSize.x), TILE_MOVE_SPEED)
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

    private void OnDestroy()
    {
        tileManager.OnTileSelect -= TileSelect;   
        tileManager.OnTileDeselect -= TileDeselect;
    }
}
