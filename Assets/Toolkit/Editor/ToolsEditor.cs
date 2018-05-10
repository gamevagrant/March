﻿using UnityEditor;
using UnityEngine;

public class ToolsEditor : MonoBehaviour
{
    [MenuItem("Tools/Instantiate Selected")]
    public static void CreatePrefab()
    {
        var clone = Instantiate(Selection.activeObject);
    }

    [MenuItem("Tools/Instantiate Selected", true)]
    public static bool ValidateCreatePrefab()
    {
        GameObject go = Selection.activeObject as GameObject;
        if (go == null)
            return false;

        return PrefabUtility.GetPrefabType(go) == PrefabType.Prefab || PrefabUtility.GetPrefabType(go) == PrefabType.ModelPrefab;
    }

    [MenuItem("Tools/Utils/Clear all Player Preferences")]
    static void ClearPlayerPrefs()
    {
        if (EditorUtility.DisplayDialog("Delete all editor preferences.",
            "Are you sure you want to delete all the editor preferences? " +
            "This action cannot be undone.", "Yes", "No"))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    [MenuItem("Tools/Utils/Clear AssetBundle Caches")]
    static void ClearAssetBundleCaches()
    {
        Caching.ClearCache();
    }
}
