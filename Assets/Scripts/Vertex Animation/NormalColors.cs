using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalColors : MonoBehaviour
{
    [SerializeField] private Mesh mesh;

    [ContextMenu("Normals to Colors")]
    void NormalsToColors()
    {
        List<Color> colors = new List<Color>();
        
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            Vector3 normal = Vector3.zero;
            int count = 0;

            // for (int j = 0; j < mesh.vertexCount; j++)
            // {
            //     if (mesh.normals[i] == mesh.normals[j])
            //     {
            //         normal += mesh.normals[j];
            //         ++count;
            //     }
            // }

            normal /= count;
            normal = (mesh.normals[i] + Vector3.one) * 0.5f;
            colors.Add(new Color(normal.x, normal.y, normal.z, 1));
        }
        
        mesh.SetColors(colors);
    }
}
