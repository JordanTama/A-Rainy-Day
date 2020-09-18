using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelSize))]
public class LevelSizeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelSize levelSize = (LevelSize) target;
        if(GUILayout.Button("Resize Level"))
        {
            levelSize.SetLevelSize();
        }
    }
}
