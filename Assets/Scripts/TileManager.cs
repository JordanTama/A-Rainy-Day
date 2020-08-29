﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : IGameService
{
    public GameObject CurrentTile { get; private set; }
    public Action<GameObject> OnTileSelect;
    public Action OnTileDeselect;

    private CameraManager cameraManager;
    private InputManager inputManager;
    private GameLoopManager _gameLoopManager;

    private bool _canMoveTiles;


    public TileManager(CameraManager camMan, InputManager input, GameLoopManager gameLoopMan)
    {
        cameraManager = camMan;
        cameraManager.OnCameraRaycastHit += TileSelect;

        inputManager = input;
        inputManager.P_LeftClick.canceled += TileDeselect;

        _gameLoopManager = gameLoopMan;
        _gameLoopManager.OnPreparation += EnableTileMove;
        _gameLoopManager.OnExecution += DisableTileMove;
        EnableTileMove();
    }

    private void TileDeselect(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (CurrentTile == null)
            return;

        CurrentTile.layer = 0;
        CurrentTile = null;
        OnTileDeselect?.Invoke();
    }

    public void TileSelect(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Tile") && CurrentTile == null && _canMoveTiles)
        {
            CurrentTile = hit.collider.gameObject;
            CurrentTile.layer = 2;
            OnTileSelect?.Invoke(CurrentTile);
        }
    }

    private void EnableTileMove()
    {
        _canMoveTiles = true;
    }
    
    private void DisableTileMove()
    {
        _canMoveTiles = false;
    }
    
    

}
