using System.Collections.Generic;
using UnityEngine;

public partial class Loader
{
    public static List<Sprite> LoadAllSpriteFromAssetBundle(string rootName, int NumberOfFrame)
    {
        List<Sprite> result = new List<Sprite>();

        if (!AssetBundleManager.Instance.GetAssetBundle(rootName, out AssetBundle bundle))
        {
            Debug.LogError($"[Loader] Không tìm thấy AssetBundle với key: {rootName}");
            return result;
        }


            string spriteName = $"{rootName}";
            Sprite sprite = bundle.LoadAsset<Sprite>(spriteName);
            if (sprite != null)
            {
                result.Add(sprite);
            }
            else
            {
                Debug.LogWarning($"[Loader] Không tìm thấy sprite: {spriteName} trong bundle {rootName}");
            }
        return result;
    }
}
