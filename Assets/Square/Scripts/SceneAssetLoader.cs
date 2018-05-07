using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace March.Scene
{
    public class SceneAssetLoader : MonoBehaviour
    {
        public bool Editable;

        public Dictionary<string, List<PrefabInfo>> SceneMap = new Dictionary<string, List<PrefabInfo>>();

        private void Start()
        {
            foreach (var path in Enum.GetNames(typeof(SceneLevel.StuffType)))
            {
                LoadAllPrefabsAsync(path);
            }
        }

        private void LoadAllPrefabsAsync(string path)
        {
            var assetBundleName = string.Format("scene/{0}", path.ToLower());

            if (!SceneMap.ContainsKey(path))
                SceneMap.Add(path, new List<PrefabInfo>());

            // Get the asset.
            var prefabList = Core.ResourceManager.ResourceManager.instance.LoadAll<Object>(assetBundleName);
            SceneMap[path].Clear();
            SceneMap[path].AddRange(prefabList.Select(prefab => new PrefabInfo { Name = prefab.name, Prefab = prefab as GameObject }));
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