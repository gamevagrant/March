using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

public class AssetsManager : MonoBehaviour {

    private const int RECOVERY_TIME = 120;//这个时间后没有被请求就自动销毁
    private Dictionary<string, CacheObject> dicCacheObject = new Dictionary<string, CacheObject>();
    private Queue<Action> queue = new Queue<Action>();
    private bool isLoading = false;

    private IAssetsLoader localAssetLoader;//加载本地资源
    private IAssetsLoader netAssetLoader;//加载网络资源
    private IAssetsLoader loader;

    private static AssetsManager _instance;
    public static AssetsManager Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = GameObject.Find("MainManager");
                
                if(go == null)
                {
                    go = new GameObject("MainManager");
                }
                GameObject.DontDestroyOnLoad(go);
                go.AddComponent<AssetsManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        //localAssetLoader = new AssetBundleLoader();
        localAssetLoader = new ResourceLoader();
        netAssetLoader = new AssetLoader();

    }


    // Update is called once per frame
    void Update () {
        if (queue.Count > 0 && !isLoading)
        {
            Action act = queue.Dequeue();
            if (act != null)
            {
                act();
            }
            
        }
    }

    /// <summary>
    /// 加载一个本地资源（file）
    /// </summary>
    /// <param name="path">相对路径</param>
    public void LoadAssetAsync<T>(string url, Action<T> callback) where T : Object
    {
        string path = url;
        path = FilePathTools.normalizePath(path);

        CacheObject co;
        if (callback != null && dicCacheObject.TryGetValue(path, out co) && co != null)
        {
            callback(co.obj as T);
            co.time = Time.time;
        }
        else
        {
            queue.Enqueue(() =>
            {
                TryClearCache();
                
                StartCoroutine(LoadAsync<T>(path,callback));
            });
        }
    }

    public T LoadAsset<T>(string path) where T : Object
    {
        return loader.LoadAsset<T>(path);
    }

    public void LoadAssetWithWWW(string url, Action<WWW> callback)
    {
        string path = url;
        path = FilePathTools.normalizePath(path);
        queue.Enqueue(() =>
        {
            TryClearCache();
            StartCoroutine(LoadWithWWW(url,callback));
        });
    }

    private IEnumerator LoadAsync<T>(string path,Action<T> callback)where T:Object
    {
        isLoading = true;
        CacheObject co;
        if (callback != null && dicCacheObject.TryGetValue(path, out co) && co != null)
        {
            callback(co.obj as T);
            co.time = Time.time;
            isLoading = false;
            yield break;
        }
        if(path.IndexOf("http://")>=0 || path.IndexOf(Application.streamingAssetsPath)>=0)
        {
            loader = netAssetLoader;
        }else
        {   
            loader = localAssetLoader;
        }
        yield return loader.LoadAssetAsync<T>(path,(res)=> {
            AddCache(path, res);
            callback(res);
            Debug.Log("加载数据："+path);
        });
        isLoading = false;
    }

    private IEnumerator LoadWithWWW(string url, Action<WWW> callback)
    {
        isLoading = true;
        WWW www = new WWW(url);
        yield return www;
        callback(www);
        isLoading = false;
    }

    private void AddCache(string path, Object obj)
    {
        if (!dicCacheObject.ContainsKey(path))
        {
            dicCacheObject.Add(path, new CacheObject(obj, Time.time));
        }
    }
    private void TryClearCache()
    {
        List<string> list = new List<string>(dicCacheObject.Keys);

        foreach (string key in list)
        {
            CacheObject co = dicCacheObject[key];
            if (Time.time - co.time > RECOVERY_TIME)
            {
                dicCacheObject.Remove(key);
            }

        }

    }

    private class CacheObject
    {

        public Object obj;
        public float time;

        public CacheObject(Object obj, float time)
        {
            this.obj = obj;
            this.time = time;

        }
    }
}
