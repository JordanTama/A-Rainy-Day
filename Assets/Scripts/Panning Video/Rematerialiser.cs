using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rematerialiser : MonoBehaviour
{
    public Material material;
    
    [ContextMenu("Rematerialise")]
    void Rematerialise()
    {
        foreach (Transform child in transform)
        {
            foreach (Transform building in child)
            {
                MeshRenderer rend = building.GetComponent<MeshRenderer>();
                if (rend)
                {
                    rend.material = material;
                }
            }
        }
    }
}
