using UnityEditor;
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
}
