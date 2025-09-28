using System.Collections.Generic;
using System.Linq;
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
        // 1. Check cache trước
        if (CachedSprites.TryGetValue(spriteName, out var cachedSprite))
        {
            return cachedSprite;
        }

        // 2. Lấy bundle
        if (!GetAssetBundle(bundleName, out var bundle) || bundle == null)
        {
            Debug.LogError($"[AssetBundleManager] Bundle '{bundleName}' not found.");
            return null;
        }

        // 3. Load tất cả Sprite (bỏ qua Texture2D)
        var sprites = bundle.LoadAllAssets<Sprite>();
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError($"[AssetBundleManager] No sprites found in bundle '{bundleName}'.");
            return null;
        }

        // 4. Add vào cache
        foreach (var s in sprites)
        {
            if (!CachedSprites.ContainsKey(s.name))
            {
                CachedSprites[s.name] = s;
            }

            // Nếu đúng tên cần thì return luôn
            if (s.name == spriteName)
            {
                return s;
            }
        }

        // 5. Nếu không có sprite khớp → log cảnh báo
        Debug.LogWarning($"[AssetBundleManager] Sprite '{spriteName}' not found in bundle '{bundleName}'. " +
                         $"Available: {string.Join(", ", sprites.Select(x => x.name))}");
        return null;
    }

}
