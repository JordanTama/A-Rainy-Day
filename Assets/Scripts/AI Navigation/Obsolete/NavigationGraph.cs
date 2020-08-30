using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NavigationGraph : MonoBehaviour
{
    [Header("Runtime Settings")]
    [SerializeField] private bool liveUpdate;
    
    private NavigationNode[] _navigationNodes;


    private void Update()
    {
        if (!liveUpdate) return;

        _navigationNodes = GetComponentsInChildren<NavigationNode>();

        foreach (NavigationNode node in _navigationNodes)
        {
            node.UpdateNode();
        }
    }

    public void ApplyNaming()
    {
        for (int i = 0; i < _navigationNodes.Length; i++)
        {
            _navigationNodes[i].gameObject.name = "Nav Node " + i;
        }
    }
}
