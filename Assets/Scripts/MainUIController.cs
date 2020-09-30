using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIController : MonoBehaviour
{
    public void RotateCamera(int dir)
    {
        ServiceLocator.Current.Get<CameraManager>().RotateCamera(dir);
    }
}
