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
        public string Name;
        public string BundleID;
        public string Version;
        public bool IsForDev;
    }

    private static BuildTarget target = BuildTarget.Android;
    private const string BuildName = "march";
    private const string BundleID = "com.elex.march";
    private const string ConfigName = "BuildConfig.txt";

    private static BuildConfig DefaultBuildConfig = new BuildConfig
    {
        Name = BuildName,
        BundleID = BundleID,
        Version = PlayerSettings.bundleVersion,
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
        var buildName = string.Format("./package/{0}_{1}_{2}{3}.apk", config.Name, config.Version, DateTime.Now.ToString(@"yyyyMMdd"), config.IsForDev ? "_dev" : "");
        BuildOptions additionOption = BuildOptions.None;
        
        if (config.IsForDev)
        {
            additionOption |= BuildOptions.ConnectWithProfiler | BuildOptions.Development;
        }

        PlayerSettings.bundleVersion = config.Version;
        PlayerSettings.productName = config.Name;
        PlayerSettings.applicationIdentifier = config.BundleID;
        PlayerSettings.companyName = config.BundleID;

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
