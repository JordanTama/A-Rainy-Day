using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dolly : MonoBehaviour
{
    public float speed;
    public Vector3 axis;
    public bool active;
    
    void Update()
    {
        if (active)
            transform.Translate(axis.normalized * (speed * Time.deltaTime), Space.World);
    }
}
