using UnityEngine;
using System.Collections;

public class GameSetting {


    //------------------全平台一致的配置--------------------------
    public static string serverPath = "http://123.206.90.153:9933";
    public static string serverPathDevelop = "http://10.1.7.215:9933";
    //public static string serverPathDevelop = "http://10.1.36.91:9933";//勇江

    public static string shareFinishTaskLink = "https://csll.app.link/ShareFinishTask";
    public static string homePage = "https://www.facebook.com/caishenlaile";
    public static bool isRelease = true;//是否时发布版本 是使用正式服务器还是测试服务器
    public const int TUTORIAL_MAX = 18;//新手教程的最大值

    //-------------------平台区分的配置-------------------------
#if UNITY_EDITOR
    public static string appID = "1342990120";
    public static string updateLookupUrl = "https://itunes.apple.com/lookup";
#elif UNITY_ANDROID
    public static string appID = "1342990120";
    public static string updateLookupUrl = "https://itunes.apple.com/lookup";
#elif UNITY_IPHONE
    public static string appID = "1342990120";
    public static string updateLookupUrl = "https://itunes.apple.com/lookup";
#endif

#if UNITY_EDITOR
    public static readonly bool isUseAssetBundle =  true;//true：使用assetbundle包中的资源,资源有变化需要重新打包，false：使用编辑器里的资源，资源更改随时生效
    public static readonly bool isUseLocalAssetBundle = true;//是否使用本地資源 使用本地资源会去streamAsset文件夹下加载，不使用会从网络加载

#else
	public static readonly bool isUseAssetBundle = true;//这条勿动，发布平台下永远是true
    public static readonly bool isUseLocalAssetBundle = true;//是否使用本地資源 使用本地资源会去streamAsset文件夹下加载，不使用会从网络加载

#endif


}
