using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Object = UnityEngine.Object;

public class CacheManager 
{
	private string cacheFileRoot = Application.persistentDataPath + "/Cache/Pic/";
	private Dictionary<string, string> cache = new Dictionary<string, string>();

	private static CacheManager _instance;

	public static CacheManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new CacheManager();
				//Debug.Log("cachePath=" + _instance.cacheFileRoot);
			}
			return _instance;
		}
	}

	public CacheManager()
	{
		DirectoryInfo dir = new DirectoryInfo(cacheFileRoot);
		if (!dir.Exists)
		{
			dir.Create();
		}
		FileInfo[] files = dir.GetFiles();
		foreach (FileInfo file in files)
		{
			cache.Add(file.Name,"file://"+file.FullName);
		}
	}
    public void AddCache(string url, byte[] bytes)
    {
        string fileName = BestHTTP.Extensions.Extensions.CalculateMD5Hash(url);
       
        if (!cache.ContainsKey(fileName))
		{
            //Debug.Log("缓存文件:" + url + " 到 " + cacheFileRoot + fileName);
            try
			{
				string path = cacheFileRoot + fileName;
				FileStream fs = new FileStream(path, FileMode.Create);
				fs.Write(bytes, 0, bytes.Length);
				fs.Flush();
				fs.Close();
				cache.Add(fileName, path);

			}
			catch (IOException e)
			{

				Debug.LogError(e);
			}
		}
		
		
		
	}

	public string GetLocalPath(string url)
	{
		string fileName = BestHTTP.Extensions.Extensions.CalculateMD5Hash(url);
		string path;
		if (cache.TryGetValue(fileName, out path))
		{
            //Debug.Log("获取缓存文件:"+url+" | " +path);
			return path;
		}
		return path;
	}

	public bool TryLoadCache<T>(string url,out T data)where T:Object
	{
		data = null;
		byte[] bytes;
		if (TryLoadCache(url,out bytes))
		{
			if (typeof(T) == typeof(Texture2D))
			{
				Texture2D tex = new Texture2D(4,4);
				tex.LoadImage(bytes);
				data = tex as T;
				
			}

			return true;
		}
		return false;
	}

	public bool TryLoadCache(string url,out byte[] bytes)
	{
		bytes = null;
		string fileName = BestHTTP.Extensions.Extensions.CalculateMD5Hash(url);
		string path;
		if (cache.TryGetValue(fileName, out path))
		{
			try
			{
				FileStream fs = new FileStream(path, FileMode.Open);
				bytes = new byte[fs.Length];
				fs.Seek(0, SeekOrigin.Begin);
				fs.Read(bytes, 0, (int)fs.Length);
				fs.Flush();
				fs.Close();
			}
			catch (IOException e)
			{
				
				Debug.LogError(e);
			}
			
			return true;
		}
		return false;
	}


}
