using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CookingToolPanelUIHandler : MonoBehaviour
{
    [SerializeField] GameObject Content;
    [SerializeField] TextMeshProUGUI ToolName;
    [SerializeField] TextMeshProUGUI ToolUse;
    [SerializeField] Transform output;
    [SerializeField] Slider progress;

    Coroutine progressRoutine;

    public void SetProp(string toolname, string tooluse)
    {
        ToolName.text = toolname;
        ToolUse.text = tooluse;
    }

    public void ReturnItemToHolder(List<IndrePrefabs> pool, Transform defaultHolder)
    {
        foreach (Transform child in Content.transform)
        {
            foreach (Transform item in child)
            {
                var prefab = item.GetComponent<IndrePrefabs>();
                if (prefab != null)
                {
                    // Đưa item về holder gốc
                    item.SetParent(defaultHolder, false);

                    // Thêm vào pool
                    pool.Add(prefab);

                    // Disable tạm, Refresh sẽ bật lại khi cần
                    prefab.gameObject.SetActive(false);
                }
            }
        }

        // Clear output
        foreach (Transform item in output)
        {
            Destroy(item.gameObject);
        }
    }

    public void OnCookingProcessSucceed()
    {
        foreach (Transform child in Content.transform)
        {
            foreach (Transform item in child)
            {
                var prefab = item.GetComponent<IndrePrefabs>();
                if (prefab != null)
                {
                    Destroy(prefab.gameObject);
                }
            }
        }
    }
    public void ClearOutput()
    {
        foreach (Transform item in output)
        {
            Destroy(item.gameObject);
        }
    }
    public int[] GetInput()
    {
        int[] result = new int[3];
        int count = 0;
        foreach (Transform child in Content.transform)
        {
            foreach (Transform item in child)
            {
                ObjectInfo iteminfo = item.GetComponent<ObjectInfo>();
                if (iteminfo != null)
                {
                    result[count] = iteminfo.ID;
                    count++;
                }
            }
        }
        return result;
    }

    public void SetOutput(GameObject obj)
    {
        obj.transform.SetParent(output, false);
    }
    public void RunProgress(float duration, Action onComplete)
    {
        // Dừng coroutine cũ nếu đang chạy
        if (progressRoutine != null)
        {
            StopCoroutine(progressRoutine);
        }
        progressRoutine = StartCoroutine(ProgressCoroutine(duration, onComplete));
    }

    private IEnumerator ProgressCoroutine(float duration, Action onComplete)
    {
        progress.value = 0f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            progress.value = Mathf.Clamp01(time / duration);
            yield return null;
        }

        progress.value = 1f;

        onComplete?.Invoke();

        progressRoutine = null;
    }
    public void SliderToglle(bool on)
    {
        progress.gameObject.SetActive(on);
    }
}
