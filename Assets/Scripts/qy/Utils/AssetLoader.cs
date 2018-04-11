using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

/// <summary>
/// 本地和网络资源的加载器，不能用以加载AssetBundle资源，要加载AssetBundle请使用AssetBundleLoadManager。
/// </summary>
public class AssetLoader:IAssetsLoader
{

	public IEnumerator LoadAssetAsync<T>(string url,Action<T> callback) where T:Object
	{
        string path = url;

        string localPath = CacheManager.instance.GetLocalPath(url);
        if (!string.IsNullOrEmpty(localPath))
        {
            path = localPath;
        }
        //Debug.Log("==开始使用WWW下载==:" + path);

        path = FilePathTools.normalizePath(path);
		WWW www = new WWW(path);
		yield return www;
		if (string.IsNullOrEmpty(www.error))
		{
			object res;
            Type type =typeof(T);
            if(type == typeof(Texture2D))
            {
                Texture2D tex = new Texture2D(4, 4);
                www.LoadImageIntoTexture(tex);
                res = tex;
            }else if (type == typeof(AssetBundle))
            {
                res = www.assetBundle;
            }
            else if (type == typeof(string))
            {
                res = www.text;
            }
            else
            {
                res = www.bytes;
            }

            CacheManager.instance.AddCache(url, www.bytes);

            www.Dispose();
            callback((T)res);
			
		}
		else
		{
            Debug.Log(url);
			Debug.Log(www.error);
            www.Dispose();
        }

    }

    public T LoadAsset<T>(string path)where T:Object
    {
        Debug.LogError("网络资源不能使用同步加载，起使用IEnumerator LoadAsync<T>(string url,Action<T> callback)");
        return null;
    }
}
