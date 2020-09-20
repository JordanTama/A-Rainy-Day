using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientTowardsCamera : MonoBehaviour
{
    private Camera _cam;
    [SerializeField] private Vector3 lookDir;

    private void Awake()
    {
        _cam = Camera.main;
        lookDir = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CheckLookDir())
        {
            lookDir = -_cam.transform.forward;
            lookDir = new Vector3(lookDir.x,0f,lookDir.z);
            transform.LookAt(lookDir,-Vector3.up);
            transform.forward = lookDir;
        }
        
    }

    bool CheckLookDir()
    {
        return lookDir == -_cam.transform.forward;
    }
}
