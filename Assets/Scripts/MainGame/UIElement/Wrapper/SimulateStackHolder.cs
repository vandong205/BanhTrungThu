using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SimulateStackHolder : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI count;
    [SerializeField] bool HideCount = false;
    public Action<ObjectInfo> _removeCallback;
    public Action<ObjectInfo> _addCallback;

    private int itemCount = 0;
    private void Awake()
    {
        if(HideCount) count.gameObject.SetActive(false);    
    }
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
        if (itemCount == 0)
        {
            info.ID = 0;
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
        if (HideCount) return;
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
    public void SetHideCount(bool hide)
    {
        HideCount = hide;
    }
}
