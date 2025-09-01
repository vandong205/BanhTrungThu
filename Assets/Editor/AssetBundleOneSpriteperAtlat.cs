using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class BuildSpriteBundles : EditorWindow
{
    private string inputPath = "Assets/Animation";
    private string outputPath = "AssetBundles";
    private BuildTarget platform = BuildTarget.StandaloneWindows64;

    [MenuItem("Tools/Build Sprite Bundles")]
    public static void ShowWindow()
    {
        GetWindow<BuildSpriteBundles>("Build Sprite Bundles");
    }

    void OnGUI()
    {
        GUILayout.Label("Build Sprite Bundles", EditorStyles.boldLabel);

        inputPath = EditorGUILayout.TextField("Input Folder", inputPath);
        if (GUILayout.Button("Browse Input"))
        {
            string absolutePath = EditorUtility.OpenFolderPanel("Select Input Folder", inputPath, "");
            if (!string.IsNullOrEmpty(absolutePath))
            {
                string relativePath = GetRelativeAssetsPath(absolutePath);
                if (!string.IsNullOrEmpty(relativePath))
                {
                    inputPath = relativePath;
                }
                else
                {
                    UnityEngine.Debug.LogWarning("Selected folder is outside the Assets directory.");
                }
            }
        }

        outputPath = EditorGUILayout.TextField("Output Folder", outputPath);
        if (GUILayout.Button("Browse Output"))
        {
            string absolutePath = EditorUtility.OpenFolderPanel("Select Output Folder", outputPath, "");
            if (!string.IsNullOrEmpty(absolutePath))
            {
                string relativePath = GetRelativeAssetsPath(absolutePath);
                if (!string.IsNullOrEmpty(relativePath))
                {
                    outputPath = relativePath;
                }
                else
                {
                    outputPath = absolutePath; // Allow output outside Assets
                }
            }
        }

        platform = (BuildTarget)EditorGUILayout.EnumPopup("Platform", platform);

        if (GUILayout.Button("Build"))
        {
            BuildBundles();
        }
    }

    private string GetRelativeAssetsPath(string absolutePath)
    {
        string assetsPath = Application.dataPath; // e.g., F:/Unity/Cursed Village1.0/Assets
        if (absolutePath.StartsWith(assetsPath))
        {
            return "Assets" + absolutePath.Substring(assetsPath.Length);
        }
        return string.Empty;
    }

    void BuildBundles()
    {
        if (!AssetDatabase.IsValidFolder(inputPath))
        {
            UnityEngine.Debug.LogError("The input path must be a valid folder within the Assets directory.");
            return;
        }

        string[] subfolders = AssetDatabase.GetSubFolders(inputPath);
        if (subfolders.Length == 0)
        {
            UnityEngine.Debug.LogWarning("No subfolders found in the selected folder.");
            return;
        }

        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();

        foreach (string subfolder in subfolders)
        {
            string bundleName = Path.GetFileName(subfolder);

            string[] guids = AssetDatabase.FindAssets("t:sprite", new[] { subfolder });

            List<string> assetPaths = new List<string>();
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                if (sprite != null)
                {
                    assetPaths.Add(assetPath);
                }
            }

            if (assetPaths.Count > 0)
            {
                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = bundleName + ".bundle";
                build.assetNames = assetPaths.ToArray();
                builds.Add(build);
            }
            else
            {
                UnityEngine.Debug.LogWarning($"No sprites found in subfolder: {subfolder}");
            }
        }

        if (builds.Count == 0)
        {
            UnityEngine.Debug.LogError("No bundles to build.");
            return;
        }

        string platformOutputPath = Path.Combine(outputPath, platform.ToString());
        Directory.CreateDirectory(platformOutputPath);

        // Use BuildAssetBundleOptions.None to avoid including dependencies automatically
        BuildPipeline.BuildAssetBundles(platformOutputPath, builds.ToArray(), BuildAssetBundleOptions.None, platform);

        UnityEngine.Debug.Log($"Asset bundles built successfully to: {platformOutputPath}");
        AssetDatabase.Refresh();
    }
}