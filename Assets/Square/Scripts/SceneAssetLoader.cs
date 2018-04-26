using AssetBundles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace March.Scene
{
    public class SceneAssetLoader : MonoBehaviour
    {
        public bool Editable;

        public const string AssetBundlesOutputPath = "/AssetBundles/";

        public Dictionary<string, List<PrefabInfo>> SceneMap = new Dictionary<string, List<PrefabInfo>>();

        // Use this for initialization
        public IEnumerator Load()
        {
            yield return StartCoroutine(Initialize());

            foreach (var path in Enum.GetNames(typeof(SceneLevel.StuffType)))
            {
                yield return StartCoroutine(LoadAllPrefabsAsync(path));
            }
        }

        // Initialize the downloading URL.
        // eg. Development server / iOS ODR / web URL
        void InitializeSourceURL()
        {
            // If ODR is available and enabled, then use it and let Xcode handle download requests.
#if ENABLE_IOS_ON_DEMAND_RESOURCES
        if (UnityEngine.iOS.OnDemandResources.enabled)
        {
            AssetBundleManager.SetSourceAssetBundleURL("odr://");
            return;
        }
#endif
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            // With this code, when in-editor or using a development builds: Always use the AssetBundle Server
            // (This is very dependent on the production workflow of the project.
            //      Another approach would be to make this configurable in the standalone player.)
            AssetBundleManager.SetSourceAssetBundleURL(Application.streamingAssetsPath + "/AssetBundles/");
            //AssetBundleManager.SetDevelopmentAssetBundleServer();
            return;
#else
        // Use the following code if AssetBundles are embedded in the project for example via StreamingAssets folder etc:
        AssetBundleManager.SetSourceAssetBundleURL(Application.streamingAssetsPath + "/AssetBundles/");
        // Or customize the URL based on your deployment or configuration
        //AssetBundleManager.SetSourceAssetBundleURL("http://www.MyWebsite/MyAssetBundles");
        return;
#endif
        }

        // Initialize the downloading url and AssetBundleManifest object.
        protected IEnumerator Initialize()
        {
            // Don't destroy this gameObject as we depend on it to run the loading script.
            DontDestroyOnLoad(gameObject);

            InitializeSourceURL();

            // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
            var request = AssetBundleManager.Initialize();
            if (request != null)
                yield return StartCoroutine(request);
        }

        protected IEnumerator LoadAllPrefabsAsync(string path)
        {
            // This is simply to get the elapsed time for this phase of AssetLoading.
            var startTime = Time.realtimeSinceStartup;

            var assetBundleName = string.Format("scene/{0}", path.ToLower());

            // Load asset from assetBundle.
            var request = AssetBundleManager.LoadAllAssetAsync(assetBundleName, typeof(GameObject));
            if (request == null)
                yield break;
            yield return StartCoroutine(request);

            if (!SceneMap.ContainsKey(path))
                SceneMap.Add(path, new List<PrefabInfo>());

            // Get the asset.
            var prefabList = request.GetAsset<Object>();
            SceneMap[path].Clear();
            SceneMap[path].AddRange(prefabList.Select(prefab => new PrefabInfo { Name = prefab.name, Prefab = prefab as GameObject }));

            // Calculate and display the elapsed time.
            var elapsedTime = Time.realtimeSinceStartup - startTime;
            Debug.Log(assetBundleName + (prefabList.Length == 0 ? " was not" : " was") + " loaded successfully in " + elapsedTime + " seconds");
        }

        public GameObject InstancePrefab(string path, int index)
        {
            return Instantiate(SceneMap[path][index].Prefab);
        }

        public GameObject CreateIdentifyGameObject(SceneLayerInfo parent, int index, Vector3 position)
        {
            var go = InstancePrefab(parent.name, index);
            go.transform.parent = parent.transform;
            go.transform.position = parent.Modifiable ? position : go.transform.position;
            go.transform.localRotation = Quaternion.identity;
            //go.transform.localScale = parent.Modifiable ? Vector3.one : go.transform.localScale;
            go.layer = parent.gameObject.layer;

            if (Editable)
            {
                var itemController = Instantiate(Resources.Load<GameObject>("ModifiableItem")).GetComponent<ItemActionController>();
                itemController.transform.parent = go.transform;
                itemController.transform.localPosition = Vector3.zero;
                itemController.Path = parent.name;
                itemController.Index = index;
                itemController.IsModifiable = parent.Modifiable;
                itemController.SceneEditor = parent.GetComponentInParent<SceneEditor>();
            }
            return go;
        }
    }
}