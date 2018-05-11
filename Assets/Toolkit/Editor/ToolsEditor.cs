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

    [MenuItem("Tools/EditorPrefs/Clear all Player Preferences")]
    static void DeleteAllExample()
    {
        if (EditorUtility.DisplayDialog("Delete all editor preferences.",
            "Are you sure you want to delete all the editor preferences? " +
            "This action cannot be undone.", "Yes", "No"))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    [MenuItem("Tools/Guide/Show Selection Path")]
    public static void ShowSelectedPath()
    {
        Debug.Log(GetPath(Selection.activeTransform));
    }

    private static string GetPath(Transform tf)
    {
        if (tf == null)
            return " null ";
        string path = tf.name;
        if (tf.parent != null && tf.parent.gameObject.GetComponent<Canvas>() == null)
        {
            path = GetPath(tf.parent) + "/" + path;
        }
        return path;
    }
}
