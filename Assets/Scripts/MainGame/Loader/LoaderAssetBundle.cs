
using System.Collections;
using System.IO;
using UnityEngine;

public partial class Loader
{
    public static IEnumerator LoadAssetBundle(string subpath, string key)
    {
        string path = Path.Combine(Application.streamingAssetsPath, subpath);
#if UNITY_ANDROID && !UNITY_EDITOR
    path = "jar:file://" + path;
    UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(path);
    yield return request.SendWebRequest();

    if (request.result != UnityWebRequest.Result.Success)
    {
        Debug.LogError(request.error);
        yield break;
    }
    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
#else
        AssetBundle bundle = AssetBundle.LoadFromFile(path);
        if (bundle == null)
        {
            Debug.LogError($"Failed to load AssetBundle from {path}");
            yield break;
        }
#endif

        if (!AssetBundleManager.Instance.IsBundleCached(key))
        {
            AssetBundleManager.Instance.AddAssetBundle(key, bundle);
        }
    }

    public static IEnumerator LoadAssetBundleInPack(string name)
    {
        yield return null;
        //foreach (BundlePack pack in Loader.bundlePacks)
        //{
        //    if (pack.Name == name)
        //    {
        //        if (pack.BundlePaths != null)
        //        {
        //            foreach (Bundle bundle in pack.BundlePaths)
        //            {
        //                if (!string.IsNullOrEmpty(bundle.Path) && !string.IsNullOrEmpty(bundle.Key))
        //                {
        //                    yield return Loader.LoadAssetBundle(bundle.Path, bundle.Key);
        //                }
        //            }
        //        }
        //        else continue;
        //        Debug.Log($"Loaded {AssetBundleManager._CachedAssetBundleDic.Count} asset bundles, latest from {pack.Name}");
        //        yield break;
        //    }
        //}
    }

}