using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class TrafficLights:InteractableReceiver
{
    private NavMeshModifierVolume blocker;
    private MeshRenderer myMesh;
    private MaterialPropertyBlock block;

    private bool isOpen;

    private void Awake()
    {
        blocker = GetComponent<NavMeshModifierVolume>();
        block = new MaterialPropertyBlock();
        myMesh = GetComponent<MeshRenderer>();
        ResetState();
    }
    

    protected override void ChangeState()
    {

        if(!isOpen) Open();
        else Close();
        base.ChangeState();
    }

    protected override void ResetState()
    {
        base.ResetState();
        Close();
    }

    private void Close()
    {
        isOpen = false;
        blocker.area = 1;
        ChangeColor(Color.red);
    }

    private void Open()
    {
        isOpen = true;
        blocker.area = 0;
        ChangeColor(Color.green);
    }

    private void ChangeColor(Color c)
    {
        block.SetColor("_Color",c);
        myMesh.SetPropertyBlock(block);
    }
}
