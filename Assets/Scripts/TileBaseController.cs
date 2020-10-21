using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;

public class TileBaseController : MonoBehaviour
{
    const float TILE_MOVE_SPEED = 0.5f;
    const float TILE_DIST_CHECK = 0.1f;

    public static Action<bool> OnTileClick;

    [SerializeField] private float _dotCheck = 0.9f;
    [SerializeField] private bool _isFixed = false;

    private TileController[] _allTiles;
    private TileManager tileManager;
    private CameraManager cameraManager;
    private Vector3 startMousePos;
    private float _tileSize = 5;
    private bool tweening;
    private Renderer[] childRenderers;
    private Shader hightlightShader;
    private AudioSource audioSource;

    static float ShaderLocalTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        _allTiles = GetComponentsInChildren<TileController>();

        tileManager = ServiceLocator.Current.Get<TileManager>();
        cameraManager = ServiceLocator.Current.Get<CameraManager>();

        tileManager.OnTileSelect += TileSelect;
        tileManager.OnTileDeselect += TileDeselect;
        childRenderers = GetComponentsInChildren<Renderer>();

        hightlightShader = Shader.Find("StandardWithHighlight");
    }

    void TileDeselect()
    {
        startMousePos = Vector3.zero;
        ShaderLocalTime = 0;
        foreach (TileController t in _allTiles)
        {
            t.gameObject.layer = 0;

        }
        foreach (Renderer r in childRenderers)
        {
            if (r.material.shader == hightlightShader)
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                block.SetColor("_HighlightColor", Color.black);
                r.SetPropertyBlock(block);
            }
        }
    }

    void TileSelect(GameObject g)
    {
        if (g == gameObject)
        {
            foreach (TileController t in _allTiles)
            {
                t.gameObject.layer = 2;
            }
            
            startMousePos = cameraManager.worldSpaceMousePos;
            OnTileClick?.Invoke(_isFixed);

            foreach (Renderer r in childRenderers)
            {
                if (r.material.shader == hightlightShader)
                {
                    MaterialPropertyBlock block = new MaterialPropertyBlock();
                    block.SetColor("_HighlightColor", _isFixed ? Color.red * 0.25f : Color.white);
                    r.SetPropertyBlock(block);
                }
            }
        }
    }

    private void Update()
    {
        // This can all be moved into some MouseMoved function later on
        if (tileManager.CurrentTile == gameObject)
        {
            ShaderLocalTime += Time.deltaTime * 3;
            Shader.SetGlobalFloat("LocalTime", ShaderLocalTime);

            if (_isFixed)
                return;

            if (Vector3.Distance(startMousePos, cameraManager.worldSpaceMousePos) > (_tileSize * TILE_DIST_CHECK) && !tweening)
            {
                var dir = (cameraManager.worldSpaceMousePos - startMousePos).normalized;
                //newPos += (dir.normalized);
                if (CheckDot(dir))
                {
                    if (CheckAdjacent(dir))
                    {
                        transform.DOMove((transform.position + (dir * _tileSize)).RoundToNearest(_tileSize), TILE_MOVE_SPEED)
                            .OnStart(() =>
                            {
                                tweening = true;
                                tileManager.TileMoving();
                            })
                            .OnComplete(() =>
                            {
                                startMousePos = cameraManager.worldSpaceMousePos;
                                tweening = false;
                                tileManager.NewTilePosition();
                            });
                    }
                }
            }
        }
    }

    bool CheckAdjacent(Vector3 dir)
    {
        foreach (TileController t in _allTiles)
        {
            if (!t.CheckAdjacent(dir))
                return false;
        }
        return true;
    }

    bool CheckDot(Vector3 dir)
    {
        if (Mathf.Abs(Vector3.Dot(dir, transform.forward)) >= _dotCheck)
            return true;

        if (Mathf.Abs(Vector3.Dot(dir, transform.right)) >= _dotCheck)
            return true;

        return false;
    }

    private void OnDestroy()
    {
        tileManager.OnTileSelect -= TileSelect;
        tileManager.OnTileDeselect -= TileDeselect;
    }
}
