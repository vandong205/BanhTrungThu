using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CartManager : MonoBehaviour
{
    [SerializeField] GameObject ItemPrefabs;
    [SerializeField] Transform content;
    [SerializeField] TextMeshProUGUI totalmoney;
    private List<ShopItemInfo> items = new List<ShopItemInfo>();
    private void Awake()
    {
        SetTotalMoneyUI();
    }
    public void AddToCart(ShopItemInfo info)
    {
        if (ExistInCart(info.ID))
        {
            ShopItemInfo item = getItem(info.ID);
            if (item != null)
                item.GetComponent<BuyItem>()?.AddOneItem();
        }
        else
        {
            if (ResourceManager.Instance.IngredientDict.TryGetValue(info.ID, out Ingredient ingre))
            {
                if (ingre != null)
                {
                    Sprite icon = AssetBundleManager.Instance.GetSpriteFromBundle("nguyenlieu", ingre.RoleName);
                    if (icon != null)
                    {
                        GameObject obj = Instantiate(ItemPrefabs, content); // parent = content
                        obj.GetComponent<BuyItem>().SetProp(icon,ingre.Price);
                        obj.GetComponent<BuyItem>()._removeCallback += SetTotalMoneyUI;
                        obj.GetComponent<BuyItem>()._DestroyCallback += RemoveFromCart;
                        ShopItemInfo shopItem = obj.GetComponent<ShopItemInfo>();
                        shopItem.SetInfo(ingre.ID, ingre.Price);

                        items.Add(shopItem);
                    }
                }
            }
        }
    }


    private bool ExistInCart(int id)
    {
        foreach (ShopItemInfo item in items)
        {
            if (item.ID == id) return true;
        }
        return false;
    }

    private ShopItemInfo getItem(int id)
    {
        foreach (ShopItemInfo item in items)
        {
            if (item.ID == id) return item;
        }
        return null;
    }
    public long getTotalMoney()
    {
        long totalMoney = 0;
        foreach (Transform child in content)
        {
            BuyItem item = child.GetComponent<BuyItem>();
            if (item != null)
            {
                
                totalMoney += item.getTotalPrice();
            }
        }
        return totalMoney;
    }
    public void SetTotalMoneyUI()
    {
        totalmoney.text = MoneyFormatConvert.FormatCurrency(getTotalMoney(), "VND");
    }
    private void RemoveFromCart(int id)
    {
        ShopItemInfo needToRemove = null;

        foreach (ShopItemInfo item in items)
        {
            if (item.ID == id)
            {
                needToRemove = item;
                break; 
            }
        }

        if (needToRemove != null)
        {
            items.Remove(needToRemove);
            Destroy(needToRemove.gameObject); 
            SetTotalMoneyUI(); 
        }
    }
    public void BuyOnclick()
    {
        ResourceManager.Instance.player.Capital-=getTotalMoney();   
        UIGamePlayManager.Instance.LoadPlayerStat();
        items.Clear();
        foreach (Transform child in content) {
            ShopItemInfo item = child.GetComponent<ShopItemInfo>();
            SimulateStackHolder itemstack = child.GetComponent<SimulateStackHolder>();
            foreach (PlayerOwnedObject ingre in ResourceManager.Instance.player.Ingredients)
            {
                if(item.ID == ingre.ID)
                {
                    ingre.Quantity += itemstack.getCount();
                }
            }
            Destroy(child.gameObject);
        }
        KitchenRoomUIManager.Instance.LoadingPlayerStat();
        totalmoney.text = MoneyFormatConvert.FormatCurrency(0, "VND");
    }
}
