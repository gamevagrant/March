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
        public enum BuildType
        {
            Apk = 1,
            AsestBundle = 2,
            All = 4,
        }

        public string ProductName;
        public string CompanyName;
        public string ApplicationId;
        public string Version;
        public string KeyStorePath;
        public string KeyPassword;
        public string KeyAliasName;
        public string KeyAliasPassword;
        public bool IsForDev;
        public string PredefineSymbols;
        public BuildType Build;
    }

    private static BuildTarget target = BuildTarget.Android;
    private const string ConfigName = "BuildConfig.txt";

    private static readonly BuildConfig DefaultBuildConfig = new BuildConfig
    {
        ProductName = string.Empty,
        CompanyName = string.Empty,
        ApplicationId = string.Empty,
        Version = string.Empty,
        KeyStorePath = string.Empty,
        KeyPassword = string.Empty,
        KeyAliasName = string.Empty,
        KeyAliasPassword = string.Empty,
        IsForDev = false,
        PredefineSymbols = string.Empty,
        Build = BuildConfig.BuildType.Apk,
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

    [MenuItem("Tools/Build/Android")]
    public static void BuildAndroid()
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

        DoAndroidBuild(config);
    }

    /// <summary>
    /// Jenkins android build.
    /// </summary>
    /// <remarks>Pay attention to android_shell.sh</remarks>
    public static void JenkinsBuildAndroid()
    {
        var config = DefaultBuildConfig;

        config.ProductName = Environment.GetEnvironmentVariable("ProductName");
        config.CompanyName = Environment.GetEnvironmentVariable("CompanyName");
        config.ApplicationId = Environment.GetEnvironmentVariable("ApplicationId");

        config.KeyStorePath = Environment.GetEnvironmentVariable("ANDROID_KEYSTORE_NAME");
        config.KeyPassword = Environment.GetEnvironmentVariable("ANDROID_KEYSTORE_PASSWORD");
        config.KeyAliasName = Environment.GetEnvironmentVariable("ANDROID_KEYALIAS_NAME");
        config.KeyAliasPassword = Environment.GetEnvironmentVariable("ANDROID_KEYALIAS_PASSWORD");

        config.Version = Environment.GetEnvironmentVariable("Version");
        config.IsForDev = bool.Parse(Environment.GetEnvironmentVariable("IsForDev"));
        config.PredefineSymbols = Environment.GetEnvironmentVariable("PredeineSymbols");

        var assetbundleBuild = bool.Parse(Environment.GetEnvironmentVariable("BuildAssetBundle"));
        var apkBuild = bool.Parse(Environment.GetEnvironmentVariable("BuildApk"));
        config.Build = assetbundleBuild && apkBuild ? BuildConfig.BuildType.All :
            assetbundleBuild ? BuildConfig.BuildType.AsestBundle : BuildConfig.BuildType.Apk;
        var assetbunldeCommit = Environment.GetEnvironmentVariable("COMMIT_MESSAGE")
            .Contains(Environment.GetEnvironmentVariable("BUILD_AB_COMMIT"));
        var jenkinsCommit = Environment.GetEnvironmentVariable("gitlabUserName").Equals("Jenkins");
        // override config build type if build is triggered by jenkins push or an ab request push.
        config.Build = assetbunldeCommit || jenkinsCommit
            ? BuildConfig.BuildType.AsestBundle
            : config.Build;

        DoAndroidBuild(config);
    }

    private static void DoAndroidBuild(BuildConfig config)
    {
        var root = Application.platform == RuntimePlatform.OSXEditor ||
                   Application.platform == RuntimePlatform.OSXPlayer
            ? Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".android")
            : Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), ".android");
        config.KeyStorePath = Path.Combine(root, config.KeyStorePath);

        Debug.LogWarning("Key store to : " + config.KeyStorePath);

        try
        {
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
        var additionOption = BuildOptions.None;

        if (config.IsForDev)
        {
            additionOption |= BuildOptions.ConnectWithProfiler | BuildOptions.Development;
        }

        PlayerSettings.bundleVersion = config.Version;
        PlayerSettings.productName = config.ProductName;
        PlayerSettings.companyName = config.CompanyName;
        PlayerSettings.applicationIdentifier = config.ApplicationId;

        //set the internal apk version to the current unix timestamp, so this increases with every build
        PlayerSettings.Android.bundleVersionCode = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        PlayerSettings.Android.keystoreName = config.KeyStorePath;
        PlayerSettings.Android.keystorePass = config.KeyPassword;
        PlayerSettings.Android.keyaliasName = config.KeyAliasName;
        PlayerSettings.Android.keyaliasPass = config.KeyAliasPassword;

        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, config.PredefineSymbols);

        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Internal;

        if (target == BuildTarget.Android)
        {
            if (config.Build == BuildConfig.BuildType.Apk || config.Build == BuildConfig.BuildType.All)
            {
                var buildName = string.Format("./package/{2}/{0}_{1}_{2}_{3}{4}.apk", config.ProductName, config.Version,
                    DateTime.Now.ToString(@"yyyyMMdd"), DateTime.Now.ToString(@"HHmmss"), config.IsForDev ? "_dev" : "");

                Debug.LogWarning("Build package to : " + new FileInfo(buildName).FullName);

                BuildPipeline.BuildPlayer(GetBuildScenes(),
                    buildName,
                    target,
                    BuildOptions.None | additionOption);
            }
            else if (config.Build == BuildConfig.BuildType.AsestBundle || config.Build == BuildConfig.BuildType.All)
            {
                Debug.LogWarning("Build asset bundles.");

                AssetBundles.BuildScript.BuildAssetBundles();
            }
            else
            {
                Debug.LogWarning("You choose nothing to build, bro!");
            }
        }
    }
}
