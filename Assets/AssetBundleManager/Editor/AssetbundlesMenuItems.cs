using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AssetBundles
{
    public class AssetBundlesMenuItems
    {
        private const string kSimulationMode = "Tools/AssetBundles/Simulation Mode";
        private const string ResourcePath = "Resources";

        [MenuItem(kSimulationMode)]
        public static void ToggleSimulationMode()
        {
            AssetBundleManager.SimulateAssetBundleInEditor = !AssetBundleManager.SimulateAssetBundleInEditor;
        }

        [MenuItem(kSimulationMode, true)]
        public static bool ToggleSimulationModeValidate()
        {
            Menu.SetChecked(kSimulationMode, AssetBundleManager.SimulateAssetBundleInEditor);
            return true;
        }

        [MenuItem("Tools/AssetBundles/Build AssetBundles from Resources")]
        private static void BuildBundlesFromResources()
        {
            var path = Path.Combine(Application.dataPath, ResourcePath);
            var assetList = Directory.GetDirectories(path).ToList()
                .Select(dir => string.Format("Assets{0}", dir.Replace(Application.dataPath, string.Empty))).ToList()
                .Select(AssetDatabase.LoadAssetAtPath<Object>).ToArray();
            BuildBundles(assetList);
        }

        [MenuItem("Tools/AssetBundles/Build AssetBundles")]
        static public void BuildAssetBundles()
        {
            BuildScript.BuildAssetBundles();
        }

        [MenuItem("Tools/AssetBundles/Build Player (for use with engine code stripping)")]
        static public void BuildPlayer()
        {
            BuildScript.BuildPlayer();
        }

        [MenuItem("Tools/AssetBundles/Build AssetBundles from Selection")]
        private static void BuildBundlesFromSelection()
        {
            BuildBundles(Selection.objects);
        }

        private static void BuildBundles(Object[] assetList)
        {
            // Get all selected *assets*
            var assets = assetList.Where(o => !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(o))).ToArray();

            List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();
            HashSet<string> processedBundles = new HashSet<string>();

            // Get asset bundle names from selection
            foreach (var o in assets)
            {
                var assetPath = AssetDatabase.GetAssetPath(o);
                var importer = AssetImporter.GetAtPath(assetPath);

                if (importer == null)
                {
                    continue;
                }

                // Get asset bundle name & variant
                var assetBundleName = importer.assetBundleName;
                var assetBundleVariant = importer.assetBundleVariant;
                var assetBundleFullName = string.IsNullOrEmpty(assetBundleVariant) ? assetBundleName : assetBundleName + "." + assetBundleVariant;

                // Only process assetBundleFullName once. No need to add it again.
                if (processedBundles.Contains(assetBundleFullName))
                {
                    continue;
                }

                processedBundles.Add(assetBundleFullName);

                AssetBundleBuild build = new AssetBundleBuild();

                build.assetBundleName = assetBundleName;
                build.assetBundleVariant = assetBundleVariant;
                build.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleFullName);

                assetBundleBuilds.Add(build);
            }

            BuildScript.BuildAssetBundles(assetBundleBuilds.ToArray());
        }
    }
}