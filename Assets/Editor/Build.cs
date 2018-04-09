using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace common
{
    class Build
    {
        static string[] SCENES = FindEnabledEditorScenes();
        static string APP_NAME = Application.productName.Replace(' ', '_');
        static string TARGET_DIR = "target";


        static string[] FindEnabledEditorScenes()
        {
            List<string> EditorScenes = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled) continue;
                EditorScenes.Add(scene.path);
            }
            return EditorScenes.ToArray();
        }

        //static string PerformAndroidBuild_Debug()
        //{
        //    string target = "_" + APP_NAME + ".apk";
        //    string dir = "./";
        //    target = dir + target;
        //    GenericBuild(SCENES, target, BuildTarget.Android, BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.ConnectWithProfiler);
        //    return target;
        //}

        //[MenuItem("CustomBuild/Build Android And Install (Debug)", priority = 1000)]
        //internal static void PerformAndroidBuild_Debug_Install()
        //{
        //    string target = PerformAndroidBuild_Debug();
        //    InstallApk(target);
        //}

        //[MenuItem("CustomBuild/Build Android And Install (Release)", priority = 1000)]
        //internal static void PerformAndroidBuild_Install()
        //{
        //    string target = "_" + APP_NAME + ".apk";
        //    string dir = "./";
        //    target = dir + target;
        //    GenericBuild(SCENES, target, BuildTarget.Android, BuildOptions.None);
        //    InstallApk(target);
        //}

        [MenuItem("CustomBuild/Build Android", priority = 1000)]
        static void PerformAndroidBuild()
        {
            string target = APP_NAME + "_"+ DateTime.Now.ToString("MM-dd_HH-mm-ss")  + ".apk";
			string target_ = CommandLineReader.GetCustomArgument("Build_Num");
			if (!String.IsNullOrEmpty (target_)) {
				target = "[" + target_ + "]" + target;
				PlayerSettings.Android.bundleVersionCode = int.Parse(target_);
			}


            string dir = "./" + TARGET_DIR + "/Android/";
            string dir_ = CommandLineReader.GetCustomArgument("Build_Dir");
            if (!String.IsNullOrEmpty(dir_))
                dir = dir_;
            dir = dir.Substring(0, dir.LastIndexOf("/"));
            Directory.CreateDirectory(dir);

            dir = dir + "/" + DateTime.Now.ToString("MM-dd");

            GenericBuild(SCENES, dir + "/" + target, BuildTarget.Android, BuildOptions.None);
        }

		static void GenericBuild(string[] scenes, string target, BuildTarget build_target, BuildOptions build_options)
		{
			if (build_target != EditorUserBuildSettings.activeBuildTarget)
			{
				throw new Exception("You need switch platform to " + build_target + " by your own, in case wrong operation.");
			}

			// Version number
			var gameVersion = CommandLineReader.GetCustomArgument("Game_Version");
			if (!string.IsNullOrEmpty(gameVersion))
			{
				PlayerSettings.bundleVersion = gameVersion;
            }


            Debug.Log("Start build " + build_target.ToString() + " with option " + build_options.ToString() + " to " + target);

            string res = BuildPipeline.BuildPlayer(scenes, target, build_target, build_options);
            if (res.Length > 0)
            {
                throw new Exception("BuildPlayer failure: " + res);
            }
        }
    }
}