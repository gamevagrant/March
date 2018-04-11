using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FilePathTools
{
	#if UNITY_ANDROID
	static string targetName = "android";
	#elif UNITY_IPHONE
	static string targetName = "iphone";
	#else
	static string targetName = "win";
	#endif
	
	public static string assetsRootPath = Application.dataPath + "/Export/";//资源文件夹绝对路径
	
	public static string exportRoot = Application.streamingAssetsPath +"/" + targetName +"/";//打包资源的输出文件夹
	
	public static string downLoadSavePath = Application.persistentDataPath + "/DownLoad/"+targetName+"/";//下载远程资源数据保存的位置

    public static string tmpPath = Application.temporaryCachePath + "/Cache/" + targetName+"/";//临时数据存放 缓存

    public static string root
	{
		get
		{
			if(!GameSetting.isUseAssetBundle)
			{
                return assetsRootPath;
			}
            /*else if(GameSetting.isUseLocalAssetBundle)
            {
                return exportRoot;
            }*/
            else
			{
				return downLoadSavePath;
			}
		}
	}


	
	//资源文件地址 只有在ios和安卓平台下才需要从远程下载，编辑器的win平台下流程不走assetbundle
	public static string downLoadRootPath
	{
		get{
			string path;
			if(GameSetting.isUseLocalAssetBundle)
			{
                #if UNITY_ANDROID
                path = exportRoot;
                #elif UNITY_IPHONE
                path = "file://" + exportRoot;
                #else
                path = "file://"+exportRoot;
                #endif

			}else
			{

				#if UNITY_IPHONE 
				path = 	"http://192.168.1.100/assets/iphone";
				#elif UNITY_ANDROID
				path = "http://192.168.1.100/assets/android";
				#else
				path = "http://192.168.1.100/assets/win";
				#endif
			}
			
			return path;
		}
		
	}
    public static string manifestPath
    {
        get
        {
            return root + targetName;
        }
    }

    public static string GetDownLoadMainVersonPath(string name)
    {
        return downLoadRootPath + targetName;
    }

    //根据一个绝对路径 获得这个资源的assetbundle name
    public static string getAssetBundleNameWithPath(string path)
	{
		string str = normalizePath (path);
		str = replaceFirst (str, root , "");
		return str;
	}

	/// <summary>
	/// 获取文件夹的所有文件，包括子文件夹 不包含.meta文件
	/// </summary>
	/// <returns>The files.</returns>
	/// <param name="path">Path.</param>
	public static FileInfo[] getFiles(string path)
	{
		DirectoryInfo folder = new DirectoryInfo (path);

		DirectoryInfo[] subFolders = folder.GetDirectories ();
		List<FileInfo> filesList = new List<FileInfo> (); 
		
		foreach(DirectoryInfo subFolder in subFolders)
		{
			filesList.AddRange(getFiles(subFolder.FullName));
		}
		
		FileInfo[] files = folder.GetFiles ();
		foreach(FileInfo file in files)
		{
			if(file.Extension != ".meta")
			{
				filesList.Add(file);
			}
			
		}
		return filesList.ToArray();
	}

	public static string[] getFilesPath(string path)
	{
		DirectoryInfo folder = new DirectoryInfo (path);
		DirectoryInfo[] subFolders = folder.GetDirectories ();
		List<string> filesList = new List<string> (); 
		
		foreach(DirectoryInfo subFolder in subFolders)
		{
			filesList.AddRange(getFilesPath(subFolder.FullName));
		}
		
		FileInfo[] files = folder.GetFiles ();
		foreach(FileInfo file in files)
		{
			if(file.Extension != ".meta")
			{
				filesList.Add(normalizePath(file.FullName));
			}
			
		}
		return filesList.ToArray();
	}
	
	//创建文件目录前的文件夹，保证创建文件的时候不会出现文件夹不存在的情况
	public static void createFolderByFilePath(string path)
	{
		FileInfo fi = new FileInfo (path);
		DirectoryInfo dir = fi.Directory;
		if(!dir.Exists)
		{
			dir.Create();
		}
	}

	public static void createFolder(string path)
	{
		DirectoryInfo dir = new DirectoryInfo(path);
		if(!dir.Exists)
		{
			dir.Create();
		}
	}
	
	//规范化路径名称 修正路径中的正反斜杠
	public static string normalizePath(string path)
	{
		return path.Replace (@"\","/");
	}
	//将绝对路径转成工作空间内的相对路径
	public static string getRelativePath(string fullPath)
	{
		string path = normalizePath (fullPath);
		//path = path.Replace(Application.dataPath,"Assets");
		path = replaceFirst (path,Application.dataPath,"Assets");
		return path;
	}
	//将相对路径转成绝对路径
	public static string getAbsolutelyPath(string relativePath)
	{
		string path = normalizePath (relativePath);
		//path = Application.dataPath.Replace("Assets","") + path;
		path = replaceFirst (Application.dataPath,"Assets","") + path;
		return path;
	}

	//替换掉第一个遇到的指定字符串
	public static string replaceFirst(string str,string oldValue,string newValue)
	{
		int i = str.IndexOf (oldValue);
		str = str.Remove (i,oldValue.Length);
		str = str.Insert (i, newValue);
		return str;
	}

    public static string getUIPath(string prefabName)
    {
        string str = "Prefabs/UI/" + prefabName;
        if (GameSetting.isUseAssetBundle)
        {
            str = str.ToLower();
        }
        else
        {
            str = str + ".prefab";
        }
        return str;
    }

    /// <summary>
    /// 获取特效prefab路径
    /// </summary>
    /// <returns>The effect path.</returns>
    /// <param name="name">Name.</param>
    public static string getEffectPath(string name)
    {
        string str = "Prefabs/Effect/" + name;
        if (GameSetting.isUseAssetBundle)
        {
            str = str.ToLower();
        }
        else
        {
            str = str + ".prefab";
        }
        return str;
    }

    public static string getScenePath(string prefabName)
	{
		string str = "Scenes/" + prefabName;
		if(GameSetting.isUseAssetBundle)
		{
			str = str.ToLower();
		}else
		{
			str = str+".unity";
		}
		return str;
	}



	public static string getConfigPath(string name)
	{
		string str = "Configs/" + name;
		if(GameSetting.isUseAssetBundle)
		{
			str = str.ToLower();
		}else
		{
			str = str+".txt";
		}
		return str;
	}

	public static string getAudioPath(string name)
	{
		string str = "Audios/" + name;
		if(GameSetting.isUseAssetBundle)
		{
			str = str.ToLower();
		}else
		{
			str = str+".mp3";
		}
		return str;
	}

    public static string getSpriteAtlasPath(string name)
    {
        string str = "SpriteAtlas/" + name;
        if (GameSetting.isUseAssetBundle)
        {
            str = str.ToLower();
        }
        else
        {
            str = str + ".spriteatlas";
        }
        return str;
    }

    public static string GetAnimationPath(string name)
    {
        string str = "Animations/" + name;
        if (GameSetting.isUseAssetBundle)
        {
            str = str.ToLower();
        }
        else
        {
            str = str + ".prefab";
        }
        return str;
    }

    public static string GetStorySpritePath(string name)
    {
        string str = "Story/" + name;
        if (GameSetting.isUseAssetBundle)
        {
            str = str.ToLower();
        }
        else
        {
            str = str + ".png";
        }
        return str;
    }

    public static string GetPersonHeadPath(string name)
    {
        string str = "Sprites/Cookie/UI/General/" + name;
        if (GameSetting.isUseAssetBundle)
        {
            str = str.ToLower();
        }
        else
        {
            str = str + ".png";
        }
        return str;
    }

}
