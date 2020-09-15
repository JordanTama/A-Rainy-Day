using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class TrafficLights:InteractableReceiver
{
    private NavMeshObstacle _obstacle;
    private MeshRenderer myMesh;
    private MaterialPropertyBlock block;

    public bool isOpenDefault;
    
    private bool isOpen;

    private void Awake()
    {
        _obstacle = GetComponent<NavMeshObstacle>();
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
        if (isOpenDefault)
        {
            Open();
        }
        else
        {
            Close();    
        }
        
    }

    private void Close()
    {
        isOpen = false;
        _obstacle.enabled = true;
        _obstacle.carving = true;
        ChangeColor(Color.red);
        transform.localScale = new Vector3(transform.localScale.x,1f,transform.localScale.z);
    }

    private void Open()
    {
        isOpen = true;
        _obstacle.carving = false;    
        _obstacle.enabled = false;    
        ChangeColor(Color.green);
        transform.localScale = new Vector3(transform.localScale.x,0.03f,transform.localScale.z);
    }

    private void ChangeColor(Color c)
    {
        block.SetColor("_Color",c);
        myMesh.SetPropertyBlock(block);
    }
}
