﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileBaseController : MonoBehaviour
{
    const float TILE_MOVE_SPEED = 0.5f;

    [SerializeField] private float _dotCheck = 0.9f;
    [SerializeField] private bool _isFixed = false;

    private TileController[] _allTiles;
    private TileManager tileManager;
    private CameraManager cameraManager;
    private Vector3 startMousePos;
    private float _tileSize = 5;
    private bool tweening;
    private Renderer[] childRenderers;

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
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.SetColor("_HighlightColor", Color.black);
            r.SetPropertyBlock(block);
        }
    }

    void TileSelect(GameObject g)
    {
        if (g == gameObject && !_isFixed)
        {
            foreach (TileController t in _allTiles)
            {
                t.gameObject.layer = 2;
            }
            
            startMousePos = cameraManager.worldSpaceMousePos;


            foreach (Renderer r in childRenderers)
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                block.SetColor("_HighlightColor", Color.yellow);
                r.SetPropertyBlock(block);
            }
        }
    }

    private void Update()
    {
        if (_isFixed)
            return;

        // This can all be moved into some MouseMoved function later on
        if (tileManager.CurrentTile == gameObject)
        {
            ShaderLocalTime += Time.deltaTime * 3;
            Shader.SetGlobalFloat("LocalTime", ShaderLocalTime);
            if (Vector3.Distance(startMousePos, cameraManager.worldSpaceMousePos) > (_tileSize * 0.5f) && !tweening)
            {
                var dir = (cameraManager.worldSpaceMousePos.RoundToNearest(_tileSize) - startMousePos).normalized;
                //newPos += (dir.normalized);
                if (CheckDot(dir))
                {
                    if (CheckAdjacent(dir))
                    {
                        transform.DOMove((transform.position + (dir * _tileSize)).RoundToNearest(_tileSize), TILE_MOVE_SPEED)
                            .OnStart(() => tweening = true)
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