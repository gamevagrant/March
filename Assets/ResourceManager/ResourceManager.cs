using AssetBundles;
using Assets.Scripts.Common;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace March.Core.ResourceManager
{
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        public bool EnableAssetBundle;

        public T Load<T>(string bundle, string asset = "") where T : Object
        {
            if (EnableAssetBundle)
            {
                string error;
                var loaded = AssetBundleManager.GetLoadedAssetBundle(bundle, out error);
                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogError(error + " in bundle: " + bundle);
                    return null;
                }
                return loaded.m_AssetBundle.LoadAsset<T>(asset);
            }

            return Resources.Load<T>(bundle);
        }

        public List<T> LoadAll<T>(string bundle) where T : Object
        {
            if (EnableAssetBundle)
            {
                string error;
                var loaded = AssetBundleManager.GetLoadedAssetBundle(bundle, out error);
                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogError(error + " in bundle: " + bundle);
                    return null;
                }
                return loaded.m_AssetBundle.LoadAllAssets<T>().ToList();
            }

            return Resources.LoadAll<T>(bundle).ToList();
        }
    }
}