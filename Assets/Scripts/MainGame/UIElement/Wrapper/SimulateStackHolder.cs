using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimulateStackHolder : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI count;

    public Action<ObjectInfo> _removeCallback;
    public Action<ObjectInfo> _addCallback;

    private int itemCount = 0;

    public void AddOneItem()
    {
        ObjectInfo info = GetComponent<ObjectInfo>();
        if (info == null)
        {
            Debug.LogWarning("Không tìm thấy ObjectInfo trên GameObject!");
            return;
        }

        itemCount++;
        _addCallback?.Invoke(info);
        UpdateCountUI();
    }

    public void RemoveOneItem()
    {
        ObjectInfo info = GetComponent<ObjectInfo>();
        if (info == null)
        {
            Debug.LogWarning("Không tìm thấy ObjectInfo trên GameObject!");
            return;
        }

        if (itemCount > 0)
        {
            itemCount--;
            _removeCallback?.Invoke(info);
        }
        UpdateCountUI();
    }

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void SetItemCount(int number)
    {
        itemCount = number;
        UpdateCountUI();
    }

    private void UpdateCountUI()
    {
        if (!icon.gameObject.activeSelf || !count.gameObject.activeSelf)
        {
            icon.gameObject.SetActive(true);
            count.gameObject.SetActive(true);
        }

        if (itemCount == 0)
        {
            icon.gameObject.SetActive(false);
            count.gameObject.SetActive(false);
        }

        count.text = itemCount.ToString();
    }
    public int getCount()
    {
        return itemCount;
    }
}
