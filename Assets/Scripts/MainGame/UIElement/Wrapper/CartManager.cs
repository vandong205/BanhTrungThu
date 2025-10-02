using System.Collections.Generic;
using UnityEngine;

public class CartManager : MonoBehaviour
{
    [SerializeField] GameObject ItemPrefabs;
    [SerializeField] Transform content;

    private List<ShopItemInfo> items = new List<ShopItemInfo>();

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
                        obj.GetComponent<BuyItem>().SetProp(icon, ingre.Price);

                        ShopItemInfo shopItem = obj.GetComponent<ShopItemInfo>();
                        shopItem.SetInfo(ingre.ID, ingre.Price);

                        items.Add(shopItem);
                    }
                }
            }
        }
    }

    //public void RemoveFromCart(int id)
    //{
    //    ShopItemInfo item = getItem(id);
    //    if (item != null)
    //    {
    //        BuyItem buy = item.GetComponent<BuyItem>();
    //        buy?.RemoveOneItem();

    //        if (item.GetComponent<SimulateStackHolder>().getCount() <= 0)
    //        {
    //            items.Remove(item);
    //            Destroy(item.gameObject); // xóa UI
    //        }
    //    }
    //}

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
}
