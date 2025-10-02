using TMPro;
using UnityEngine;

public class BuyItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TotalText;
    [SerializeField] SimulateStackHolder itemstack;
    [SerializeField] ShopItemInfo iteminfo;
    public void AddOneItem()
    {
        Debug.LogWarning("Da them 1 item vao gia hang");
        itemstack.AddOneItem();
        TotalText.text = MoneyFormatConvert.FormatCurrency(iteminfo.Price * itemstack.getCount(),"VND");
    }
    public void RemoveOneItem()
    {
        itemstack.RemoveOneItem();
        TotalText.text = MoneyFormatConvert.FormatCurrency(iteminfo.Price * itemstack.getCount(), "VND");
    }
    public void SetProp(Sprite icon,long price)
    {
        itemstack.SetIcon(icon);
        itemstack.SetItemCount(1);
        TotalText.text = price.ToString();
    }
    public long getTotalPrice()
    {
        return itemstack.getCount() * iteminfo.Price;
    }
}
