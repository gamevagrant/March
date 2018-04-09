using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

[System.Serializable]
public class MyTexture
{
	public string TextAssetPath;
	public Texture2D TextureRef;
}

public class ResourceManager : MonoBehaviour 
{
	public MyTexture[] ReplacementTextures;

    void Awake()
    {
        StartCoroutine("ReplaceTextures");
    }

	IEnumerator ReplaceTextures()
    {
		string prefix = "";

		if (Screen.height < 700) 
        {
			prefix = "360p/";
		}

		for (int i = 0; i < ReplacementTextures.Length; i++) 
        {
			ResourceRequest request = Resources.LoadAsync<TextAsset>(prefix + ReplacementTextures[i].TextAssetPath);

			yield return request;

			if (request != null) {
				yield return ReplacementTextures[i].TextureRef.LoadImage((request.asset as TextAsset).bytes);
			}
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(ResourceManager))]
public class ResourceManagerEditor : Editor
{
	private ReorderableList _ReplacementTextures;
	private void OnEnable()
	{
		_ReplacementTextures = new ReorderableList(serializedObject, serializedObject.FindProperty("ReplacementTextures"), true, true, true, true);

		_ReplacementTextures.drawHeaderCallback = (Rect rect) => {
			EditorGUI.LabelField(rect, "Replacement Texture List");
		};

		_ReplacementTextures.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			SerializedProperty element = _ReplacementTextures.serializedProperty.GetArrayElementAtIndex(index);
			SerializedProperty textAssetPath = element.FindPropertyRelative("TextAssetPath");
			SerializedProperty textureRef = element.FindPropertyRelative("TextureRef");
			if (textAssetPath.stringValue == "") {
				textAssetPath.stringValue = "TextAssetPath";
			}

			EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width * 0.5f - 10, rect.height), textAssetPath, GUIContent.none);
			EditorGUI.PropertyField(new Rect(rect.x + rect.width * 0.5f, rect.y, rect.width * 0.5f, rect.height), textureRef, GUIContent.none);
		};
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		_ReplacementTextures.DoLayoutList();

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Select All", GUILayout.Width(150)))
		{
			Object[] selectionObjects = new Object[_ReplacementTextures.count];
			for (int i = 0; i < _ReplacementTextures.count; i++)
			{
				SerializedProperty element = _ReplacementTextures.serializedProperty.GetArrayElementAtIndex(i);
				selectionObjects[i] = element.FindPropertyRelative("TextureRef").objectReferenceValue as Texture2D;
			}
			Selection.objects = selectionObjects;
		}
		GUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();
	}
}
#endif