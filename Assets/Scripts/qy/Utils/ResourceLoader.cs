using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader:IAssetsLoader
{

    public IEnumerator LoadAssetAsync<T>(string path, System.Action<T> callback) where T : Object
    {
        ResourceRequest rq = Resources.LoadAsync<T>(path);
        yield return rq;
        if(rq.isDone)
        {
            callback(rq.asset as T);
        }
    }


    public T LoadAsset<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }
}
