using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private AIManager manager;

    private void Awake()
    {
        manager.Initialize();
    }
}
