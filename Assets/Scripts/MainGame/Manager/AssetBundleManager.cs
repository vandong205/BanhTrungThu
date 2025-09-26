using System.Collections.Generic;
using UnityEngine;

public class AssetBundleManager : MonoBehaviour
{
    private static AssetBundleManager instance;
    public static AssetBundleManager Instance;

    public Dictionary<string, AssetBundle> CachedAssetBundle = new Dictionary<string, AssetBundle>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Kiểm tra xem bundle đã cache chưa
    public bool IsBundleCached(string bundleName)
    {
        return CachedAssetBundle.ContainsKey(bundleName);
    }

    // Thêm bundle vào cache
    public void AddAssetBundle(string bundleName, AssetBundle bundle)
    {
        if (!IsBundleCached(bundleName))
        {
            CachedAssetBundle.Add(bundleName, bundle);
            Debug.Log($"[AssetBundleManager] Đã add bundle: {bundleName}");
        }
    }

    // Lấy bundle từ cache
    public bool GetAssetBundle(string bundleName, out AssetBundle bundle)
    {
        return CachedAssetBundle.TryGetValue(bundleName, out bundle);
    }

    // Xóa bundle khỏi cache nhưng không Unload
    public void RemoveAssetBundle(string bundleName)
    {
        if (CachedAssetBundle.ContainsKey(bundleName))
        {
            CachedAssetBundle.Remove(bundleName);
            Debug.Log($"[AssetBundleManager] Đã remove bundle: {bundleName}");
        }
    }

    // Unload 1 bundle (có thể chọn giữ asset đã load hay không)
    public void UnloadAssetBundle(string bundleName, bool unloadAllLoadedObjects = false)
    {
        if (CachedAssetBundle.TryGetValue(bundleName, out AssetBundle bundle))
        {
            bundle.Unload(unloadAllLoadedObjects);
            CachedAssetBundle.Remove(bundleName);
            Debug.Log($"[AssetBundleManager] Đã unload bundle: {bundleName}");
        }
        else
        {
            Debug.LogWarning($"[AssetBundleManager] Không tìm thấy bundle: {bundleName}");
        }
    }

    // Unload tất cả bundle
    public void UnloadAllAssetBundle(bool unloadAllLoadedObjects = false)
    {
        foreach (var kvp in CachedAssetBundle)
        {
            kvp.Value.Unload(unloadAllLoadedObjects);
        }
        CachedAssetBundle.Clear();
        Debug.Log("[AssetBundleManager] Đã unload tất cả AssetBundle");
    }
    public Dictionary<string, Sprite> CachedSprites = new();

    public Sprite GetSpriteFromBundle(string bundleName, string spriteName)
    {
        if (CachedSprites.TryGetValue(spriteName, out var sprite))
            return sprite;

        if (GetAssetBundle(bundleName, out var bundle))
        {
            foreach (var s in bundle.LoadAllAssets<Sprite>())
            {
                CachedSprites[s.name] = s;
                if (s.name == spriteName)
                    return s;
            }
        }
        return null;
    }
}
