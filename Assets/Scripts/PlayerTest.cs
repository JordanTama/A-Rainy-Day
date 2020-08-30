using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    private GameManager _gameManager;
    public float speed;
    private bool _isMove;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Start()
    {
        _gameManager = ServiceLocator.Current.Get<GameManager>();
        _gameManager.OnExecution += MoveForward;
        _gameManager.OnPreparation += Reset;
        
        _isMove = false;
        startPosition = transform.position;
        startRotation = transform.rotation;
    }




    private void Update()
    {
        if(_isMove)
        {
            transform.position+=transform.forward * (speed * Time.deltaTime);
        }
    }

    private void MoveForward()
    {
        _isMove = true;
    }
    
    private void StopMove()
    {
        _isMove = false;
    }
    
    private void Reset()
    {
        StopMove();
        transform.SetPositionAndRotation(startPosition,startRotation);
    }
    
    
}
