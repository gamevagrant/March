using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class ResourceLoadManager:Singleton<ResourceLoadManager>
    {
        public override void Init()
        {
            base.Init();
            //todo 资源路径设置等

        }

        public GameObject SpawnEffect(string path)
        {
            string epath = string.Format("Prefabs/Effects/{0}", path);
#if UNITY_EDITOR
            //Debug.LogError("加载特效:"+path);
#endif
            GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load(epath));
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            return obj;
        }

        public T SpawnUiItem<T>() where T : Component
        {
            string path = string.Format("Prefabs/UI/Item/{0}", typeof(T));
            return SpawnUi<T>(path);
        }
        public T SpawnUiItem<T>(Transform tsParent) where T : Component
        {
            string path = string.Format("Prefabs/UI/Item/{0}", typeof(T));
            return SpawnUi<T>(path, tsParent);
        }
        public T SpawnUiItem<T>(string path) where T : Component
        {
            path = string.Format("Prefabs/UI/Item/{0}", path);
            return SpawnUi<T>(path);
        }
        public T SpawnUiItem<T>(string path,Transform tsParent) where T : Component
        {
            path = string.Format("Prefabs/UI/Item/{0}", path);
            return SpawnUi<T>(path, tsParent);
        }
        public T SpawnUi<T>() where T : Component
        {
            string path = string.Format("Prefabs/UI/{0}", typeof (T));
            return SpawnUi<T>(path);
        }

        public T SpawnUi<T>(string path) where T : Component
        {
            T obj =GameObject.Instantiate(Resources.Load<T>(path));
            obj.transform.localScale = Vector3.one;
            return obj;
        }
        public T SpawnUi<T>(Transform tsParent) where T : Component
        {
            string path = string.Format("Prefabs/UI/Prefab/{0}", typeof(T));
            return SpawnUi<T>(path,tsParent);
        }
        public T SpawnUi<T>(string path,Transform tsParent) where T : Component
        {
            //Debug.Log("loading res->ui"+path);
            T obj = GameObject.Instantiate(Resources.Load<T>(path), tsParent, false);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            return obj;
        }

        public T SpawnAgent<T>(uint heroId, Transform parent) where T : Component
        {
            var path = string.Format("Prefabs/Hero/{0:000}_hero", heroId);
            T obj = GameObject.Instantiate(Resources.Load<T>(path), parent, false);
            obj.transform.localPosition = Vector3.zero;            
            return obj;
        }
        public T SpawnAgent<T>(uint heroId) where T : Component
        {
            var path = string.Format("Prefabs/Hero/{0:000}_hero", heroId);
            T obj = GameObject.Instantiate(Resources.Load<T>(path));
            obj.transform.localPosition = Vector3.zero;
            return obj;
        }
    }
}
