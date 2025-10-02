using System;
using TMPro;
using UnityEngine;

public class BuyItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TotalText;
    [SerializeField] GameObject x;
    [SerializeField] SimulateStackHolder itemstack;
    [SerializeField] ShopItemInfo iteminfo;
    public Action _removeCallback;
    public Action<int> _DestroyCallback;
    public void AddOneItem()
    {
        if (!TotalText.gameObject.activeSelf) TotalText.gameObject.SetActive(true);
        if (!x.activeSelf) x.SetActive(true);
        itemstack.AddOneItem();
        TotalText.text = MoneyFormatConvert.FormatCurrency(iteminfo.Price * itemstack.getCount(),"VND");
    }
    public void RemoveOneItem()
    {
        itemstack.RemoveOneItem();
        if (itemstack.getCount() <= 0)
        {
            _DestroyCallback?.Invoke(iteminfo.ID);
            return;
        }
        TotalText.text = MoneyFormatConvert.FormatCurrency(iteminfo.Price * itemstack.getCount(), "VND");
        _removeCallback?.Invoke();
    }
    public void SetProp(Sprite icon,long price)
    {
        itemstack.SetIcon(icon);
        itemstack.SetItemCount(1);
        TotalText.text = MoneyFormatConvert.FormatCurrency(price,"VND");
    }
    public long getTotalPrice()
    {
        return itemstack.getCount() * iteminfo.Price;
    }
}
