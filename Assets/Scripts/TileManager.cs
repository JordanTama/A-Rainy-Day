using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        _gameLoopManager.OnComplete += DisableTileMove;
        EnableTileMove();

        SceneManager.activeSceneChanged += (s,i) => CurrentTile = null;
    }

    private void TileDeselect(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (CurrentTile == null)
            return;

        CurrentTile.layer = 0;
        CurrentTile = null;
        OnTileDeselect?.Invoke();
    }

    public void TileSelect(RaycastHit[] hits)
    {
        if (!_canMoveTiles)
            return;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Tile") && CurrentTile == null)
            {
                CurrentTile = hit.collider.gameObject;
                CurrentTile.layer = 2;
                OnTileSelect?.Invoke(CurrentTile);
            }
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
