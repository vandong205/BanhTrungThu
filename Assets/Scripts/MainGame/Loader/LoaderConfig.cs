using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public partial class Loader
{
    /// <summary>
    /// Coroutine load JSON từ Addressables và thêm vào list.
    /// Hỗ trợ JSON object hoặc mảng.
    /// </summary>
    public static IEnumerator LoadJsonConfigIntoList<T>(string assetName, List<T> targetList)
    {
        if (targetList == null)
        {
            Debug.LogError("List truyền vào không được null");
            yield break;
        }

        // Bắt đầu load asset
        var handle = Addressables.LoadAssetAsync<TextAsset>(assetName);

        // Chờ load xong
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            string json = handle.Result.text;
            try
            {
                if (json.TrimStart().StartsWith("["))
                {
                    var list = JsonConvert.DeserializeObject<List<T>>(json);
                    if (list != null)
                        targetList.AddRange(list);
                }
                else
                {
                    var obj = JsonConvert.DeserializeObject<T>(json);
                    if (obj != null)
                        targetList.Add(obj);
                }

                Debug.Log($"Loaded {targetList.Count} items into list from {assetName}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Parse JSON lỗi: {e}");
            }
        }
        else
        {
            Debug.LogError($"Không load được asset JSON: {assetName}");
        }

        // Giải phóng asset
        Addressables.Release(handle);
    }
    public static T LoadJsonObjectFromString<T>(string json)
    {
        if (string.IsNullOrEmpty(json))
            return default;

        return JsonConvert.DeserializeObject<T>(json);
    }
    public static IEnumerator LoadJsonConfigIntoDict<TKey, TValue>(
        string assetName,
        Dictionary<TKey, TValue> targetDict)
    {
        if (targetDict == null)
        {
            Debug.LogError("Dictionary truyền vào không được null");
            yield break;
        }

        var handle = Addressables.LoadAssetAsync<TextAsset>(assetName);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            string json = handle.Result.text;
            try
            {
                json = json.Trim();

                if (!json.StartsWith("{"))
                {
                    Debug.LogError("JSON không hợp lệ: phải là object { key: value }");
                    yield break;
                }

                var dict = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(json);
                if (dict != null)
                {
                    foreach (var kvp in dict)
                    {
                        if (!targetDict.ContainsKey(kvp.Key))
                            targetDict.Add(kvp.Key, kvp.Value);
                        else
                            Debug.LogWarning($"Key {kvp.Key} đã tồn tại, bỏ qua item");
                    }
                }

                Debug.Log($"Loaded {targetDict.Count} items into dictionary from {assetName}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Parse JSON lỗi: {e}");
            }
        }
        else
        {
            Debug.LogError($"Không load được asset JSON: {assetName}");
        }

        Addressables.Release(handle);
    }
    public static IEnumerator ParseJson<T>(string key, System.Action<T> setter = null)
    {
        AsyncOperationHandle<TextAsset> handle = Addressables.LoadAssetAsync<TextAsset>(key);
        yield return handle; // chờ load xong

        T obj = default;
        if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
        {
            obj = JsonUtility.FromJson<T>(handle.Result.text);
        }
        else
        {
            Debug.LogError($"Không thể load config từ key: {key}");
        }

        setter?.Invoke(obj);
        yield return obj;

        // Nếu muốn giữ asset trong bộ nhớ thì không Release
        // Nếu chỉ load để parse 1 lần thì nên Release
        Addressables.Release(handle);
    }


}
