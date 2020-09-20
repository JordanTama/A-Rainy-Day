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
    public Action OnNewTilePosition;
    public Action OnRebakeMesh;

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

        SceneManager.activeSceneChanged += SceneChanged;
    }

    private void SceneChanged(Scene arg0, Scene arg1)
    {
        CurrentTile = null;
        _canMoveTiles = true;
    }

    private void TileDeselect(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (CurrentTile == null)
            return;

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
                CurrentTile = hit.collider.transform.parent.gameObject;
                OnTileSelect?.Invoke(CurrentTile);
                Debug.Log(CurrentTile.name); 
            }
        }
    }

    public void NewTilePosition()
    {
        OnNewTilePosition?.Invoke();
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
