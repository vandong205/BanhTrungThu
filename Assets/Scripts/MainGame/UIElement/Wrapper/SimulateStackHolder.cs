using System;
using TMPro;
using UnityEngine;
using UnityEngine.Build.Pipeline;
using UnityEngine.UI;
public class SimulateStackHolder : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI count;
    public Action _removeCallback;
    public Action _AddCallback;
    private int itemCount = 0;
    public void RemoveOneItem()
    {
        itemCount = itemCount > 1 ? itemCount-1 : 0;
        if (itemCount > 0) _removeCallback?.Invoke();
        UpdateCountUI();
    }
    public void AddOneItem()
    {
        _AddCallback?.Invoke();
        itemCount++;
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
}
