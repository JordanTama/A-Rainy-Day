using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(NavigationGraph))]
public class NavigationGraphEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Apply Naming"))
        {
            foreach (NavigationGraph t in targets)
                t.ApplyNaming();
        }
    }
}
