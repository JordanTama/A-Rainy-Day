using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class VATGenerator : MonoBehaviour
{
    #if UNITY_EDITOR
    [SerializeField] private GameObject target;
    [SerializeField] private AnimationClip clip;
    [SerializeField] private int samples;
    [SerializeField] private Space space;
    [SerializeField] private string directory;
    
    [Header("Debug Settings")]
    [SerializeField] private Texture2D textureToPrint;

    private Mesh _mesh;
    
    private const string illegalChars = "|";
    
    [ContextMenu("Generate")]
    public void Generate()
    {
        SkinnedMeshRenderer skinnedMeshRenderer = target.transform.GetComponentInChildren<SkinnedMeshRenderer>();

        if (!skinnedMeshRenderer)
        {
            Debug.Log("Could not find MeshFilter or SkinnedMeshRenderer!");
            return;
        }

        int width = skinnedMeshRenderer.sharedMesh.vertexCount;
        
        Texture2D texture = new Texture2D(width, samples, TextureFormat.RGBAHalf, true, false);

        _mesh = new Mesh();

        for (int y = 0; y < samples; y++)
        {
            float t = y / (samples - 1.0f);
            clip.SampleAnimation(target, t * clip.length);
            
            skinnedMeshRenderer.BakeMesh(_mesh);

            for (int x = 0; x < width; x++)
            {
                Vector3 pos = _mesh.vertices[x];
                if (space == Space.World) pos = target.transform.TransformPoint(pos);
                texture.SetPixel(x, y, new Color(pos.x, pos.y, pos.z, 1));
            }
        }

        texture.Apply();

        // Save to asset database
        byte[] bytes = texture.EncodeToEXR(Texture2D.EXRFlags.None);
        
        string fileName = target.name + "-" + clip.name + ".exr";
        foreach (char c in fileName)
        {
            if (illegalChars.Contains(c.ToString())) fileName = fileName.Replace(c.ToString(), "");
        }
        File.WriteAllBytes(Application.dataPath + directory + fileName, bytes);
        UnityEditor.AssetDatabase.Refresh();

        DestroyImmediate(texture);
    }

    [ContextMenu("Print Texture")]
    public void PrintTexture()
    {
        PrintTexture(textureToPrint);
    }

    private void PrintTexture(Texture2D printTex)
    {
        for (int y = 0; y < samples; y++)
        {
            float t = y / (samples - 1.0f);
            
            String rowString = "\nFrame " + (y + 1) + " / " + printTex.height + ": \n";

            for (int x = 0; x < printTex.width; x++)
            {
                rowString += printTex.GetPixel(x, y).r + ", " + printTex.GetPixel(x, y).g +  ", " + printTex.GetPixel(x, y).b + "\n";
            }

            Debug.Log(rowString);
        }
    }
    #endif
}
