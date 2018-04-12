using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Object = UnityEngine.Object;

/// <summary>
/// 用以加载本地AssetBundle加载器，只能用于加载AssetBundle。
/// 当GameSetting.isUseAssetBundle==false时，会使用AssetDatabase.LoadAssetAtPath从项目路径直接加载资源
/// 这样在修改资源后不用重新打包就能看到资源的变化
/// </summary>
public class AssetBundleLoader:IAssetsLoader {

    
    private AssetBundleManifest manifest;
    private Object obj;


    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">资源的相对路径</param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public IEnumerator LoadAssetAsync<T>(string path, Action<T> callback) where T : Object
    {

        path = FilePathTools.normalizePath(path);
        Debug.Log("===AssetBundleLoader.loadAsync:" + path);
        string assetBundleName = path;
        path = FilePathTools.root + path;//改成绝对路径

#if UNITY_EDITOR
        bool isUseAssetBundle = GameSetting.isUseAssetBundle;
        if (!isUseAssetBundle)
        {

			path = FilePathTools.getRelativePath(path);//绝对路径转为相对Assets文件夹的相对路径
			Object Obj = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(T));


			if (Obj == null)
			{
				Debug.LogError ("Asset not found at path:" + path);
			}
            callback((T)Obj);
            yield break;
        }
#endif
        //打的ab包都资源名称和文件名都是小写的
        AssetBundleRequest assetRequest;
        AssetBundleCreateRequest createRequest;
        //1加载Manifest文件
        if (manifest == null)
        {
            string manifestPath = FilePathTools.manifestPath;
            Debug.Log("start load Manifest:" + manifestPath);
            createRequest = AssetBundle.LoadFromFileAsync(manifestPath);
            yield return createRequest;
            if (createRequest.isDone)
            {
                AssetBundle manifestAB = createRequest.assetBundle;
                yield return assetRequest = manifestAB.LoadAssetAsync<AssetBundleManifest>("AssetBundleManifest");
                manifest = assetRequest.asset as AssetBundleManifest;
                manifestAB.Unload(false);

            }
            else
            {
                Debug.Log("Manifest加载出错");
            }

        }


        //2获取文件依赖列表
        string[] dependencies = manifest.GetAllDependencies(assetBundleName);

        //3加载依赖资源
        Dictionary<string, AssetBundle> dependencyAssetBundles = new Dictionary<string, AssetBundle>();
        //Debug.Log("---开始加载依赖资源:" + dependencies.Length.ToString());
        foreach (string fileName in dependencies)
        {
            string dependencyPath = FilePathTools.root + "/" + fileName;
            Debug.Log("开始加载依赖资源:" + dependencyPath);

            createRequest = AssetBundle.LoadFromFileAsync(dependencyPath);
            yield return createRequest;
            if (createRequest.isDone)
            {
                dependencyAssetBundles.Add(dependencyPath, createRequest.assetBundle);

            }
            else
            {
                Debug.Log("加载依赖资源出错");
            }


        }
        //4加载目标资源
        obj = null;
        Debug.Log("---开始加载目标资源:" + path);
        createRequest = AssetBundle.LoadFromFileAsync(path);
        yield return createRequest;
        List<AssetBundle> abList = new List<AssetBundle>();
        if (createRequest.isDone)
        {
            AssetBundle assetBundle = createRequest.assetBundle;
            yield return assetRequest = assetBundle.LoadAssetAsync(Path.GetFileNameWithoutExtension(path), typeof(T));
            obj = assetRequest.asset;


            //5释放目标资源
            //Debug.Log("---释放目标资源:" + path);
            abList.Add(assetBundle);

        }
        else
        {
            Debug.Log("加载目标资源出错 ");
        }



        if (dependencyAssetBundles != null)
        {
            //6释放依赖资源
            foreach (string key in dependencyAssetBundles.Keys)
            {
                //Debug.Log("---释放依赖资源:" + key);
                AssetBundle dependencyAB = dependencyAssetBundles[key];
                abList.Add(dependencyAB);
            }
        }


        callback((T)obj);
        UnloadAssetbundle(abList);
        //Debug.Log("---end loadAsync:AssetBundleLoader.loadAsync" + path);
        yield return null;

    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">资源的相对路径</param>
    /// <returns></returns>
    public T LoadAsset<T>(string path) where T : Object
    {
        path = FilePathTools.normalizePath(path);
        Debug.Log("===AssetBundleLoader.Load:" + path);
        string assetBundleName = path;
        path = FilePathTools.root + path;//改成绝对路径
#if UNITY_EDITOR
        if (!GameSetting.isUseAssetBundle)
        {

            path = FilePathTools.getRelativePath(path);
            Object Obj = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(T));


            if (Obj == null)
            {
                Debug.LogError("Asset not found at path:" + path);
            }
            return (T)Obj;

        }
#endif
        //打的ab包都资源名称和文件名都是小写的

        //1加载Manifest文件
        if (manifest == null)
        {
            string manifestPath = FilePathTools.manifestPath;
            Debug.Log("start load Manifest:" + manifestPath);

            AssetBundle manifestAB = AssetBundle.LoadFromFile(manifestPath);
            manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            manifestAB.Unload(false);

        }


        //2获取文件依赖列表
        string[] dependencies = manifest.GetAllDependencies(assetBundleName);

        //3加载依赖资源
        Dictionary<string, AssetBundle> dependencyAssetBundles = new Dictionary<string, AssetBundle>();
        //Debug.Log("---开始加载依赖资源:" + dependencies.Length.ToString());
        foreach (string fileName in dependencies)
        {
            string dependencyPath = FilePathTools.root + "/" + fileName;
            Debug.Log("开始加载依赖资源:" + dependencyPath);

            dependencyAssetBundles.Add(dependencyPath, AssetBundle.LoadFromFile(dependencyPath));

        }
        //4加载目标资源
        //Object obj = null;
        Debug.Log("---开始加载目标资源:" + path);
        AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
        obj = assetBundle.LoadAsset<T>(Path.GetFileNameWithoutExtension(path)); ;

        //5释放目标资源
        //Debug.Log("---释放目标资源:" + path);
        List<AssetBundle> abList = new List<AssetBundle>();
        abList.Add(assetBundle);

        if (dependencyAssetBundles != null)
        {
            //6释放依赖资源
            foreach (string key in dependencyAssetBundles.Keys)
            {
                //Debug.Log("---释放依赖资源:" + key);
                AssetBundle dependencyAB = dependencyAssetBundles[key];
                abList.Add(dependencyAB);
            }
        }
        UnloadAssetbundle(abList);

        return (T)obj;

    }

    private void UnloadAssetbundle(List<AssetBundle> list)
    {
        //为了解决在ios上同步加载后直接释放造成加载出来的资源被回收的问题，需要隔一帧再释放
        for (int i =0;i<list.Count;i++)
        {
            list[i].Unload(false);
        }
        
    }

   


}
