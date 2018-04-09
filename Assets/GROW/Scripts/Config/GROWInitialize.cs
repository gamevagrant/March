using UnityEngine;
using System.Collections;
using System.Linq;

namespace Grow {
[ExecuteInEditMode()]
public class GROWInitialize : MonoBehaviour {

#if UNITY_EDITOR
	private class SceneModificationObserver : UnityEditor.AssetModificationProcessor
	{
		public static string[] OnWillSaveAssets(string[] paths)
		{
	#if UNITY_5_3
			var currentSceneName = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path;
	#else
			var currentSceneName = UnityEditor.EditorApplication.currentScene;
	#endif
			foreach (string s in paths) {
				if (s == currentSceneName) {
					if (GameObject.FindObjectOfType<GROWInitialize>() != null) {
						Grow.GrowEditorScript.SceneIntegratedOn = s;
						Grow.GrowEditorScript.IntegratedPrefabId = GameObject.FindObjectOfType<GROWInitialize>().GrowInitId;
					} else {
						if (currentSceneName == Grow.GrowEditorScript.SceneIntegratedOn) {
							Grow.GrowEditorScript.SceneIntegratedOn = null;
							Grow.GrowEditorScript.IntegratedPrefabId = null;
						}
					}
					break;
				}
			}
			return paths;
		}
	}
#endif

	[SerializeField] [HideInInspector]
	public string GrowInitId = null;

	// Use this for initialization
	void Start () {
		if (GrowInitId == null || GrowInitId.Length == 0) {
			GrowInitId = "GrowInit:" + this.gameObject.GetInstanceID();
		}
		if (Application.isPlaying) {
			Grow.Highway.GrowHighway.Initialize();
		}
#if UNITY_EDITOR
		else {
	#if UNITY_5_3
			var currentSceneName = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path;
	#else
			var currentSceneName = UnityEditor.EditorApplication.currentScene;
	#endif
			// looking for `GROW` GameObject is to search that now the second GROW Prefab will be added
			if (GrowEditorScript.SceneIntegratedOn != null &&
				currentSceneName != GrowEditorScript.SceneIntegratedOn &&
				GrowEditorScript.IntegratedPrefabId != null &&
				GrowEditorScript.IntegratedPrefabId != this.GrowInitId) {
				UnityEditor.EditorUtility.DisplayDialog("GROW already integrated", string.Format("GROW is already integrated in scene {0}, and must be removed before integrating into another scene.", GrowEditorScript.SceneIntegratedOn), "OK");
				if (this.gameObject.name.StartsWith("GROW")) {
					DestroyImmediate(this.gameObject);
				} else {
					DestroyImmediate(this);
				}
	#if UNITY_5_3
				UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
	#else
				UnityEditor.EditorApplication.MarkSceneDirty();
	#endif
			}
	#if !UNITY_5_0
			else if (GrowEditorScript.SceneIntegratedOn == null
		#if UNITY_5_3
						&& !UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().isDirty
		#else
			            && !UnityEditor.EditorApplication.isSceneDirty
		#endif
			) {
				GrowEditorScript.SceneIntegratedOn = currentSceneName;
				}
	#endif
			}
#endif
		}
	}
}
