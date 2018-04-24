using March.Core.WindowManager;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class WindowEditor : MonoBehaviour
{
    private const string WindowPrefabPath = "Assets/Resources/Popup";
    private const string PopupControllerPath = "Assets/Animation/UI/PopupAnimation.controller";

    [MenuItem("Tools/WindowManager/Generate Window Map")]
    public static void GenerateWindowMap()
    {
        var path = WindowPrefabPath;
        var guidList = AssetDatabase.FindAssets("popup t:GameObject", new[] { path });

        var windowMap = new WindowManagerConfig();
        guidList.ToList().ForEach(guid =>
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            windowMap.WindowList.Add(new WindowConfig
            {
                Name = string.Format("{0}Window", go.name),
                Path = assetPath.Substring("Assets/Resources/".Length, assetPath.Length - "Assets/Resources/".Length - ".prefab".Length),
            });

            GenerateWindowIdentifier(go.name);
        });

        var json = JsonUtility.ToJson(windowMap);
        var jsonPath = string.Format("Assets/Resources/{0}.json", WindowManager.MapPath);
        File.WriteAllText(jsonPath, json);

        AssetDatabase.Refresh();

        Debug.Log(string.Format("Save window manager config file to {0}", jsonPath));
    }

    private static void GenerateWindowIdentifier(string name)
    {
        var windowPath = string.Format("Assets/Scripts/Window/{0}Window.cs", name);
        if (!File.Exists(windowPath))
        {
            var windowText = string.Format("public class {0}Window : Window {{}} ", name);
            if (!File.Exists(windowText))
                File.WriteAllText(windowPath, windowText);
        }
    }

    [MenuItem("Tools/WindowManager/Prefab Windows")]
    public static void PreparePrefab()
    {
        var jsonPath = string.Format("Assets/Resources/{0}.json", WindowManager.MapPath);
        var json = File.ReadAllText(jsonPath);
        var windowManageConfigr = JsonUtility.FromJson<WindowManagerConfig>(json);
        windowManageConfigr.WindowList.ForEach(windowConfig =>
        {
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(string.Format("Assets/Resources/{0}.prefab",
                windowConfig.Path));

            // add window identifier.
            var windowName = string.Format("{0}Window", go.name);
            var windowType = Type.GetType(string.Format("{0}, Assembly-CSharp", windowName));
            var window = go.GetComponent(windowName);
            if (window == null)
                go.AddComponent(windowType);

            // add animator controller.
            var animator = go.GetComponent<Animator>();
            if (animator == null)
            {
                animator = go.AddComponent<Animator>();
            }

            if (animator.runtimeAnimatorController == null)
            {
                var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(PopupControllerPath);
                animator.runtimeAnimatorController = controller;
            }

            // add popup script.
            var popupController = go.GetComponent<Popup>();
            if (popupController == null)
                go.AddComponent<Popup>();
        });
        Debug.Log("Prepare all popup windows with 1. add animator 2. add PoppuAnimation controller 3. add window identifier 4. add popup script.");
    }
}
