using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using UnityEditor;
using UnityEngine;
using UnityEngine;

internal static class AIContextMenu
{
    [MenuItem("Assets/Create/AI Navigation/Manager")]
    static void NewManager()
    {
        AIManager manager = ScriptableObject.CreateInstance<AIManager>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        path += "/NewAINavigationManager.asset";
        
        ProjectWindowUtil.CreateAsset(manager, path);
    }
}
