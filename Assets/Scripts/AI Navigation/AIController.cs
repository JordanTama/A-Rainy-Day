using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private AIManager manager;
    [SerializeField] private bool playOnAwake;

    private void Awake()
    {
        manager.Initialize();
        if (playOnAwake) manager.Play();
    }
}
