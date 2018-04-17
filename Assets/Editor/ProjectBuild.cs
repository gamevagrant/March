using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ProjectBuild
{
    class BuildConfig
    {
        public string ProductName;
        public string CompanyName;
        public string ApplicationId;
        public string Version;
        //public int BundleVersionCode;
        public string KeyStorePath;
        public string KeyPassword;
        public string KeyAliasName;
        public bool IsForDev;
    }

    private static BuildTarget target = BuildTarget.Android;

    private const string DefaultProductName = "candy";
    private const string DefaultCompanyName = "elex";
    private const string DefaultApplicationId = "com.elex.candy";
    private const int DefaultBundleVersionCode = 1;

    private const string ConfigName = "BuildConfig.txt";

    private static readonly BuildConfig DefaultBuildConfig = new BuildConfig
    {
        ProductName = DefaultProductName,
        CompanyName = DefaultCompanyName,
        ApplicationId = DefaultApplicationId,
        //BundleVersionCode = DefaultBundleVersionCode,
        Version = PlayerSettings.bundleVersion,
        KeyStorePath = string.Empty,
        KeyPassword = string.Empty,
        KeyAliasName = string.Empty,
        IsForDev = false,
    };

    public static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();

        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;
            if (e.enabled)
            {
                if (!File.Exists(e.path))
                    continue;
                names.Add(e.path);
            }
        }
        return names.ToArray();
    }

    [MenuItem("Tools/Build/Android_Release")]
    public static void BuildRelease()
    {
        Build(false);
    }

    [MenuItem("Tools/Build/Android_Debug")]
    public static void BuildDebug()
    {
        Build(true);
    }

    private static void Build(bool isForDev)
    {
        try
        {
            var configPath = string.Format("./package/{0}", ConfigName);
            var config = DefaultBuildConfig;
            if (!File.Exists(configPath))
            {
                var str = JsonMapper.ToJson(DefaultBuildConfig);
                File.WriteAllText(configPath, str);
            }
            else
            {
                var str = File.ReadAllText(configPath);
                config = JsonMapper.ToObject<BuildConfig>(str);
            }

            config.IsForDev = isForDev;

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, target);
            if (EditorUserBuildSettings.activeBuildTarget == target)
            {
                Debug.Log("Switch Platform Success!");
                BuildPlayer(target, config);
            }
            else
            {
                Debug.LogError("Platform error!");
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    private static void BuildPlayer(BuildTarget target, BuildConfig config)
    {
        var buildName = string.Format("./package/{0}_{1}_{2}{3}.apk", config.ProductName, config.Version, DateTime.Now.ToString(@"yyyyMMdd"), config.IsForDev ? "_dev" : "");
        var additionOption = BuildOptions.None;
        
        if (config.IsForDev)
        {
            additionOption |= BuildOptions.ConnectWithProfiler | BuildOptions.Development;
        }

        PlayerSettings.bundleVersion = config.Version;
        //PlayerSettings.Android.bundleVersionCode = config.BundleVersionCode;
        PlayerSettings.productName = config.ProductName;
        PlayerSettings.companyName = config.CompanyName;
        //PlayerSettings.applicationIdentifier = string.Format("com.{0}.{1}", config.CompanyName, config.ProductName);
        PlayerSettings.applicationIdentifier = config.ApplicationId;

        PlayerSettings.Android.keystoreName = config.KeyStorePath;
        PlayerSettings.Android.keystorePass = config.KeyPassword;
        PlayerSettings.Android.keyaliasName = config.KeyAliasName;
        PlayerSettings.Android.keyaliasPass = config.KeyPassword;

        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Internal;

        if (target == BuildTarget.Android)
        {
            BuildPipeline.BuildPlayer(GetBuildScenes(),
                buildName,
                target,
                BuildOptions.None | additionOption);
        }
    }
}
