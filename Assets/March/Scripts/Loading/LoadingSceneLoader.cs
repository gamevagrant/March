using AssetBundles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace March.Scene
{
    public class LoadingSceneLoader : MonoBehaviour
    {
        public List<string> BundleList;

        private void Awake()
        {
            BundleList.Add(Configure.ConfigurePath);
            BundleList.Add(Configure.LevelBundlePath);
            BundleList.Add(Configure.FilmBackgroundPath);
            BundleList.Add(Configure.SceneBackgroundPath);
            BundleList.Add(Configure.SceneBuildingPath);
            BundleList.Add(Configure.SceneNPCPath);
            BundleList.Add(Configure.ScenePlayerPath);
            BundleList.Add(Configure.StoryBackground);
            BundleList.Add(Configure.StoryPerson);
        }

        public IEnumerator Load()
        {
            yield return StartCoroutine(Initialize());

            foreach (var path in BundleList)
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
#if ENABLE_BUNDLE_SERVER
            // Use the following code if AssetBundles are embedded in the project for example via StreamingAssets folder etc:
            //AssetBundleManager.SetSourceAssetBundleURL(Application.streamingAssetsPath + "/AssetBundles/");
            // Or customize the URL based on your deployment or configuration
            AssetBundleManager.SetSourceAssetBundleURL(string.Format("{0}/AssetBundles", Configure.instance.AssetBundleServerUrl));
            return;
#else
            // With this code, when in-editor or using a development builds: Always use the AssetBundle Server
            // (This is very dependent on the production workflow of the project.
            //      Another approach would be to make this configurable in the standalone player.)
            AssetBundleManager.SetSourceAssetBundleURL(string.Format("{0}/AssetBundles",
                Application.streamingAssetsPath));
            //AssetBundleManager.SetDevelopmentAssetBundleServer();
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

        protected IEnumerator LoadAllPrefabsAsync(string assetBundleName)
        {
            // This is simply to get the elapsed time for this phase of AssetLoading.
            var startTime = Time.realtimeSinceStartup;

            // Load asset from assetBundle.
            var request = AssetBundleManager.LoadAllAssetAsync(assetBundleName, typeof(Object));
            if (request == null)
                yield break;

            yield return StartCoroutine(request);

            // Get the asset.
            var prefabList = request.GetAsset<Object>();

            // Calculate and display the elapsed time.
            var elapsedTime = Time.realtimeSinceStartup - startTime;
            Debug.Log(assetBundleName + (prefabList.Length == 0 ? " was not" : " was") + " loaded successfully in " + elapsedTime + " seconds");
        }
    }
}