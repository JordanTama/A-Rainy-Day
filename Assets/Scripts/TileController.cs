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
    private float _tileSize = 5;

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
        SetStartPosition();
    }

    void TileSelect(GameObject g)
    {
        if (g == gameObject)
        {
            startMousePos = cameraManager.worldSpaceMousePos;
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.SetColor("_HighlightColor", Color.yellow);
            gameObject.GetComponent<Renderer>().SetPropertyBlock(block);
        }
    }

    void TileDeselect()
    {
        startMousePos = Vector3.zero;
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetColor("_HighlightColor", Color.black);
        gameObject.GetComponent<Renderer>().SetPropertyBlock(block);
    }

    private void Update()
    {
        if (fixedInPlace)
            return;

        //// This can all be moved into some MouseMoved function later on
        //if (tileManager.CurrentTile == gameObject)
        //{
        //    if (Vector3.Distance(startMousePos, cameraManager.worldSpaceMousePos) > (_tileSize * 0.5f) && !tweening)
        //    {
        //        var dir = (cameraManager.worldSpaceMousePos.RoundToNearest(_tileSize) - transform.position).normalized;
        //        //newPos += (dir.normalized);
        //        if (CheckDot(dir))
        //        {
        //            if (CheckAdjacent(dir))
        //            {
        //                transform.DOMove((transform.position + (dir * _tileSize)).RoundToNearest(_tileSize), TILE_MOVE_SPEED)
        //                    .OnStart(() => tweening = true)
        //                    .OnComplete(() =>
        //                    {
        //                        startMousePos = cameraManager.worldSpaceMousePos;
        //                        tweening = false;
        //                        tileManager.NewTilePosition();
        //                    });
        //            }
        //        }
        //    }
        //}
    } 

    bool CheckDot(Vector3 dir)
    {
        if (Mathf.Abs(Vector3.Dot(dir, transform.forward)) > 0.75f)
            return true;

        if (Mathf.Abs(Vector3.Dot(dir, transform.right)) > 0.75f)
            return true;

        return false;
    }

    public bool CheckAdjacent(Vector3 dir)
    {
        Debug.DrawRay(transform.position, dir.normalized * 2.55f);
        RaycastHit hit;
        if(Physics.BoxCast(transform.position, new Vector3(2f, 0.5f, 2f), dir.normalized, out hit, Quaternion.identity, 2.5f))
        {
            if (!hit.transform.IsChildOf(transform.parent))
            {
                return false;
            }
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
        tileManager.OnRebakeMesh?.Invoke();
    }

    private void OnDestroy()
    {
        _gameLoopManager.OnRestart -= ResetPosition;
        tileManager.OnTileSelect -= TileSelect;   
        tileManager.OnTileDeselect -= TileDeselect;
    }
}
