/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Grow
{
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	/// <summary>
	/// This class holds the store's configurations.
	/// </summary>
	public class GrowEditorScript : ScriptableObject
	{
		private static string HighwaySettingsPrefix = "Highway";

		static string currentModuleVersion = "3.1.8";

		const string growSettingsAssetName = "GrowEditorScript";
		const string soomSettingsPath = "GROW/Resources";
		const string soomSettingsAssetExtension = ".asset";

#if UNITY_EDITOR
		static GrowEditorScript() {

			var integrations =
				from assembly in System.AppDomain.CurrentDomain.GetAssemblies ()
				from type in assembly.GetTypes ()
				where type.IsSubclassOf (typeof(Grow.Integrations.SourceCodePatcherGrowIntegration))
				select type;

			foreach (var integration in integrations) {
				Grow.Integrations.SourceCodePatcherGrowIntegration integrationInstance = integration.GetProperty ("Instance", 
																					  System.Reflection.BindingFlags.Static |	System.Reflection.BindingFlags.NonPublic
																							).GetValue (null, null) as Grow.Integrations.SourceCodePatcherGrowIntegration;
				Dictionary<string, string>[] patchingInstructions = integrationInstance.GetPatchingInstructions();
				char[] delims = { '|' } ;
				for (int inst = 0; inst < patchingInstructions.Length; inst++) {
					Dictionary<string, string> patchingInstruction = patchingInstructions [inst];

					string defaultPath = patchingInstruction[Grow.Integrations.SourceCodePatcherGrowIntegration.DEFAULT_PATH_KEY];
					string nameSpace = patchingInstruction[Grow.Integrations.SourceCodePatcherGrowIntegration.NAMESPACE_KEY];
					string className = patchingInstruction[Grow.Integrations.SourceCodePatcherGrowIntegration.CLASS_NAME_KEY];
					string[] modifiers = patchingInstruction[Grow.Integrations.SourceCodePatcherGrowIntegration.METHOD_MODIFIERS_KEY].Split(delims, System.StringSplitOptions.RemoveEmptyEntries);
					string methodName = patchingInstruction[Grow.Integrations.SourceCodePatcherGrowIntegration.METHOD_NAME_KEY];
					string[] argTypes = patchingInstruction[Grow.Integrations.SourceCodePatcherGrowIntegration.METHOD_ARG_TYPES_KEY].Split(delims, System.StringSplitOptions.RemoveEmptyEntries);
					string codeToInsert = patchingInstruction[Grow.Integrations.SourceCodePatcherGrowIntegration.CODE_TO_INSERT_KEY];

					patchScript (defaultPath, nameSpace, className, modifiers, methodName, argTypes, codeToInsert);
				}
			}
			
		}
#endif

		private static void patchScript(string defaultPath,
										string nameSpace,
										string className,
										string[] methodModifiers,
										string methodName,
										string[] methodArgTypes,
										string codeToInsert) {
			char[] delimiterChars = { ' ', '\t', '\n' };
			char[] argsDelimiterChars = { ',' } ;

			string file = FindClassFile (defaultPath, nameSpace, className);
			if (File.Exists (file)) {
				string codeFile = File.ReadAllText (file);
				string code = codeFile;
				int insertionIndex = -1;
				for (int methodIndex = code.IndexOf(methodName);
					methodIndex > 0;
					methodIndex = (code = code.Substring (methodIndex + 1)).IndexOf(methodName)) {

					insertionIndex += methodIndex + 1;

					// prefix is what's before the method name
					string prefix = code.Substring (0, methodIndex);

					// suffix is what's after the method name
					string suffix = code.Substring (methodIndex + methodName.Length);

					// methodArgs will get only the method arguments, if this is indeed what we're looking for
					int offsetForInsert = suffix.IndexOf('{');
					if (offsetForInsert < 0) {
						continue;
					}
					string methodArgs = suffix.Substring(0, offsetForInsert);

					// check if the first thing after the method name is a '('
					if (!methodArgs.Trim().StartsWith ("(")) {
						continue;
					}

					string[] args = methodArgs.Replace ("(", "").Replace (")", "").Trim().Split (argsDelimiterChars, System.StringSplitOptions.RemoveEmptyEntries);

					// check that the exact amount of arguments is present
					if (args.Length != methodArgTypes.Length) {
						continue;
					}

					// make sure that the argument types match
					bool argsMatch = true;
					for (int i = 0; i < args.Length; i++) {
						if (!args [i].Trim().Split(delimiterChars)[0].Equals (methodArgTypes [i])) {
							argsMatch = false;
							break;
						}
					}
					if (!argsMatch) {
						continue;
					}

					// check that the method modifiers match
					string[] allPrefixWords = prefix.Split (delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
					bool modifiersMatch = true;
					if (allPrefixWords.Length >= methodModifiers.Length) {
						for (int i = 1; i <= methodModifiers.Length; i++) {
							if (!allPrefixWords [allPrefixWords.Length - i].Equals (methodModifiers [methodModifiers.Length - i])) {
								modifiersMatch = false;
								break;
							}
						}
						if (!modifiersMatch) {
							continue;
						}
					} else {
						continue;
					}

					insertionIndex += offsetForInsert + methodName.Length + 1;

					// prefix is what's before the inserted code
					prefix = codeFile.Substring (0, insertionIndex);

					// suffix is what's after the inserted
					suffix = codeFile.Substring (insertionIndex);

					if (!suffix.Trim ().Replace (" ", "").StartsWith (codeToInsert.Trim ().Replace (" ", ""))) {
						codeFile = prefix + "\n            " + codeToInsert + suffix;
						File.WriteAllText (file, codeFile);
					}

					break;
				}
			}
		}

		private static bool ValidateClass(string filepath, string nameSpace, string className) {
			string codeFile = File.ReadAllText (filepath);
			return codeFile.Contains ("class " + className) && codeFile.Contains ("namespace " + nameSpace);
		}

		private static string FindClassFile(string defaultPath, string nameSpace, string className)
		{
			classFiles = new List<string> ();
			string filepath = Path.Combine(Application.dataPath, defaultPath);
			if (File.Exists (filepath) && ValidateClass (filepath, nameSpace, className)) {
				return filepath;
			}

			//Lookup class name in file names
			FindAllScriptFiles(Application.dataPath);

			// First go over all files with matching name
			for (int i = 0; i < classFiles.Count; i++) {
				if (classFiles[i].Contains(className) && ValidateClass (classFiles[i], nameSpace, className)) {
					return classFiles[i];
				}
			}

			// Then go over all script files
			for (int i = 0; i < classFiles.Count; i++) {
				if (ValidateClass (classFiles[i], nameSpace, className)) {
					return classFiles[i];
				}
			}

			return null;
		}

		static List<string> classFiles;
		static void FindAllScriptFiles(string startDir)
		{
			try
			{
				foreach (string file in Directory.GetFiles(startDir))
				{
					if (file.Contains(".cs")/* || file.Contains(".js")*/)
						classFiles.Add(file);
				}
				foreach (string dir in Directory.GetDirectories(startDir))
				{
					FindAllScriptFiles(dir);
				}
			}
			catch (System.Exception ex)
			{
				Debug.Log(ex.Message);
			}
		}

		[SerializeField]
		public ObjectDictionary GrowSettings;

		public static GUILayoutOption FieldHeight = GUILayout.Height(16);
		public static GUILayoutOption FieldWidth = GUILayout.Width(120);
		public static GUILayoutOption SpaceWidth = GUILayout.Width(24);
		public static GUIContent EmptyContent = new GUIContent("");

		public static JSONObject versionJson;
		public static WWW www;

		private static GrowEditorScript instance;

		public static GrowEditorScript Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Resources.Load(growSettingsAssetName) as GrowEditorScript;

					if (instance == null)
					{
						// If not found, autocreate the asset object.
						instance = CreateInstance<GrowEditorScript>();
#if UNITY_EDITOR
						string properPath = Path.Combine(Application.dataPath, soomSettingsPath);
						if (!Directory.Exists(properPath))
						{
							AssetDatabase.CreateFolder("Assets/GROW", "Resources");
						}

						string fullPath = Path.Combine(Path.Combine("Assets", soomSettingsPath),
							growSettingsAssetName + soomSettingsAssetExtension);
						AssetDatabase.CreateAsset(instance, fullPath);
						Selection.activeObject = GrowEditorScript.Instance;
#endif
					}
				}
				return instance;
			}
		}

#if UNITY_EDITOR

		public static void OnEnable() {
			GrowManifestTools.GenerateManifest();
		}

		static GUIContent highwayGameKeyLabel = new GUIContent("Game Key: ", "The GROW Highway game key for your game");
		static GUIContent highwayEnvKeyLabel = new GUIContent("Env Key: ", "The GROW Highway environment key for your game");

		public static void OnInspectorGUI() {

			if (SceneIntegratedOn != null && !System.IO.File.Exists(SceneIntegratedOn)) {
				SceneIntegratedOn = null;
			}

			GUIStyle paddingStyle = new GUIStyle();
			paddingStyle.padding = new RectOffset(0, 20, 0, 0);

			FileStream fs = new FileStream(Application.dataPath + @"/GROW/Editor/grow_logo.png", FileMode.Open, FileAccess.Read);
			byte[] imageData = new byte[fs.Length];
			fs.Read(imageData, 0, (int)fs.Length);
			Texture2D logoTexture = new Texture2D(300, 92);
			logoTexture.LoadImage(imageData);

			EditorGUILayout.BeginHorizontal(paddingStyle);
			GUIStyle linkStyle = new GUIStyle(GUI.skin.label);
			linkStyle.normal.textColor = Color.blue;
			linkStyle.alignment = TextAnchor.MiddleRight;
			EditorGUILayout.Space();
			if (GUILayout.Button("Go to Dashboard", linkStyle, FieldHeight)) {
				Application.OpenURL("http://dashboard.soom.la");
			}
			EditorGUILayout.EndHorizontal();

			float logoHeight = 92.0f;
			GUIStyle style = new GUIStyle(paddingStyle);
			style.alignment = TextAnchor.MiddleCenter;
			style.fixedHeight = logoHeight;
			GUILayout.BeginHorizontal(logoTexture, style);
			GUILayout.Space(logoHeight);
			GUILayout.EndHorizontal();

			GameObject.DestroyImmediate(logoTexture);

			///
			/// Create Highway Game key and Env key labels and text fields
			///
			EditorGUILayout.BeginHorizontal(paddingStyle);
			EditorGUILayout.LabelField(highwayGameKeyLabel,  GUILayout.Width(90), FieldHeight);
			HighwayGameKey = EditorGUILayout.TextField(HighwayGameKey, FieldHeight);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal(paddingStyle);
			EditorGUILayout.LabelField(highwayEnvKeyLabel,  GUILayout.Width(90), FieldHeight);
			HighwayEnvKey = EditorGUILayout.TextField(HighwayEnvKey, FieldHeight);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();

			//
			// Grow version info
			//
			EditorGUILayout.BeginHorizontal(paddingStyle);

#if UNITY_5_3
			var currentSceneName = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path;
#else
			var currentSceneName = UnityEditor.EditorApplication.currentScene;
#endif
			if (SceneIntegratedOn == null || SceneIntegratedOn == currentSceneName) {
				if (GameObject.FindObjectOfType<GROWInitialize>() == null) {
					if (GUILayout.Button("Integrate GROW into current scene")) {
						GameObject growPrefab = (GameObject)Instantiate(
							(GameObject)AssetDatabase.LoadAssetAtPath("Assets/GROW/GROW.prefab", typeof(GameObject)),
							new Vector3(0, 0, 0),
							new Quaternion()
						);
						growPrefab.name = "GROW";
#if UNITY_5_3
						UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
#else
						EditorApplication.MarkSceneDirty();
#endif
					}
				} else {
					if (GUILayout.Button("Remove GROW from current scene")) {
						GameObject.DestroyImmediate(GameObject.FindObjectOfType<GROWInitialize>().gameObject);
#if UNITY_5_3
						UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
#else
						EditorApplication.MarkSceneDirty();
#endif
					}
				}
			} else {
				GUI.enabled = false;
				GUILayout.Button(new GUIContent("GROW integrated", string.Format("GROW integrated in {0}", SceneIntegratedOn)));
				GUI.enabled = true;
			}

			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal(paddingStyle);
			EditorGUILayout.LabelField("GROW Version: ", GUILayout.Width(90), FieldHeight);
			EditorGUILayout.LabelField(currentModuleVersion, GUILayout.Width(45), FieldHeight);
			LatestVersionField("unity3d-highway", currentModuleVersion, "New version available!", "http://library.soom.la/fetch/unity3d-highway/latest?cf=unity");
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			//
			// Integrations info section
			//
			EditorGUILayout.BeginVertical(paddingStyle);
			EditorGUILayout.HelpBox("GROW Integrations", MessageType.None);
			string integrationsRoot = "Assets/GROW/Scripts/Integrations";
			if (System.IO.Directory.Exists(integrationsRoot) && (new System.IO.DirectoryInfo(integrationsRoot)).GetFiles().Where(assembly => assembly.Extension.Equals(".dll")).Count() > 0) {
				foreach (FileInfo integrationsAssembly in (new System.IO.DirectoryInfo(integrationsRoot)).GetFiles().Where(assembly => assembly.Extension.Equals(".dll"))) {
					foreach (System.Type integration in System.Reflection.Assembly.LoadFile(integrationsRoot + "/" + integrationsAssembly.Name).GetTypes().Where(type => type.IsSubclassOf(typeof(Grow.Integrations.GrowIntegration)))) {
						IntegrationLabel(integration, integrationsAssembly);
					}
				}
			} else {
				foreach (System.Type integration in typeof(Grow.Integrations.GrowIntegration).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Grow.Integrations.GrowIntegration)))) {
					IntegrationLabel(integration, null);
				}
			}

			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();
		}

		public static void LatestVersionField(string moduleId, string currentVersion, string versionPrompt, string downloadLink)
		{
			if (www == null || (www.error != null && www.error.Length > 0)) {
				www = new WWW("http://library.soom.la/fetch/info");
			}
			string latestVersion = null;
			if (versionJson == null) {
				if (www.isDone) {
					versionJson = new JSONObject(www.text);
				}
				DirtyEditor();
			}
			else {
				latestVersion = versionJson.GetField (moduleId).GetField ("latest").str;
			}

			EditorGUILayout.BeginHorizontal();
			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.normal.textColor = Color.blue;
			if (GUILayout.Button ((latestVersion != null && currentVersion != latestVersion) ? versionPrompt : "", style, GUILayout.Width (170), FieldHeight)) {
				if (latestVersion != null && currentVersion != latestVersion) {
					Application.OpenURL(downloadLink);
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		public static void IntegrationLabel(System.Type integration, FileInfo assemblyFile) {
			if (integration.IsAbstract) {
				return;
			}
			Grow.Integrations.GrowIntegration intgInstance = integration.GetProperty ("Instance", System.Reflection.BindingFlags.Static |
																								  System.Reflection.BindingFlags.NonPublic
																					  ).GetValue (null, null) as Grow.Integrations.GrowIntegration;
			string integrationName = intgInstance.GetIntegrationDisplayName();
			string integrationVersion = intgInstance.GetIntegrationVersion();
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(integrationName, GUILayout.Width(90), FieldHeight);
			EditorGUILayout.LabelField(integrationVersion, GUILayout.Width(60), FieldHeight);
			if (assemblyFile != null) {
				GUIStyle style = new GUIStyle(GUI.skin.label);
				style.normal.textColor = Color.blue;
				style.alignment = TextAnchor.MiddleRight;
				EditorGUILayout.Space();
				if (GUILayout.Button("Remove", style, FieldHeight)) {
					if (EditorUtility.DisplayDialog("Remove " + integrationName + " integration", "Are you sure you want to remove " + integrationName + " integration?", "Yes", "No")) {
						string[] dependencies = intgInstance.GetDependencies();
						foreach (var dep in dependencies) {
							FileUtil.DeleteFileOrDirectory(Application.dataPath + "/" + dep);
						}
						assemblyFile.Delete();
					}
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		[MenuItem("Window/GROW Settings")]
		public static void Edit() {
			Selection.activeObject = Instance;
		}

#endif

		public static void DirtyEditor()
		{
#if UNITY_EDITOR
			EditorUtility.SetDirty(Instance);
#endif
		}

		public static string HIGHWAY_GAME_KEY_DEFAULT_MESSAGE = "[YOUR GAME KEY]";

		public static string HIGHWAY_ENV_KEY_DEFAULT_MESSAGE = "[YOUR ENV KEY]";

		private static string highwayGameKey;
		public static string HighwayGameKey
		{
			get {
				if (highwayGameKey == null) {
					highwayGameKey = GrowEditorScript.GetConfigValue(HighwaySettingsPrefix, "HighwayGameKey");
					if (highwayGameKey == null) {
						highwayGameKey = HIGHWAY_GAME_KEY_DEFAULT_MESSAGE;
					}
				}
				return highwayGameKey;
			}
			set
			{
				if (highwayGameKey != value)
				{
					highwayGameKey = value;
					GrowEditorScript.SetConfigValue(HighwaySettingsPrefix, "HighwayGameKey", value);
					GrowEditorScript.DirtyEditor ();
				}
			}
		}

		private static string highwayEnvKey;
		public static string HighwayEnvKey
		{
			get {

				if (highwayEnvKey == null) {
					highwayEnvKey = GrowEditorScript.GetConfigValue(HighwaySettingsPrefix, "HighwayEnvKey");
					if (highwayEnvKey == null) {
						highwayEnvKey = HIGHWAY_ENV_KEY_DEFAULT_MESSAGE;
					}
				}
				return highwayEnvKey;
			}
			set
			{
				if (highwayEnvKey != value)
				{
					highwayEnvKey = value;
					GrowEditorScript.SetConfigValue(HighwaySettingsPrefix, "HighwayEnvKey", value);
					GrowEditorScript.DirtyEditor ();
				}
			}
		}

		public static string IntegratedPrefabId {
			get {
				string value =  GrowEditorScript.GetConfigValue(HighwaySettingsPrefix, "IntegratedPrefabId");
				return value != null && value.Length > 0 ? value : null;
			}
			set {
				string v = GrowEditorScript.GetConfigValue(HighwaySettingsPrefix, "IntegratedPrefabId");
				if (v != value) {
					GrowEditorScript.SetConfigValue(HighwaySettingsPrefix, "IntegratedPrefabId", value == null ? "" : value);
					GrowEditorScript.DirtyEditor ();
				}
			}
		}

		public static string SceneIntegratedOn {
			get {
				string value =  GrowEditorScript.GetConfigValue(HighwaySettingsPrefix, "SceneIntegratedOn");
				return value != null && value.Length > 0 ? value : null;
			}
			set
			{
				string v = GrowEditorScript.GetConfigValue(HighwaySettingsPrefix, "SceneIntegratedOn");
				if (v != value)
				{
					GrowEditorScript.SetConfigValue(HighwaySettingsPrefix, "SceneIntegratedOn", value == null ? "" : value);
					GrowEditorScript.DirtyEditor ();
				}
			}
		}

		public static void SetConfigValue(string prefix, string key, string value) {
			PlayerPrefs.SetString("Grow." + prefix + "." + key, value);
			Instance.GrowSettings["Grow." + prefix + "." + key] = value;
			PlayerPrefs.Save();
		}

		public static string GetConfigValue(string prefix, string key) {
			string value;
			if (Instance.GrowSettings.TryGetValue("Grow." + prefix + "." + key, out value) && value.Length > 0) {
				return value;
			} else {
				value = PlayerPrefs.GetString("Grow." + prefix + "." + key);
				SetConfigValue(prefix, key, value);
				return value.Length > 0 ? value : null;
			}
		}
	}
}
