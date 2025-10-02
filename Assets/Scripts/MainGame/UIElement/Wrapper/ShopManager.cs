using UnityEngine;
using System.Collections.Generic;
public class ShopManager : MonoBehaviour
{
    [SerializeField] Transform ShopContent;
    [SerializeField] CartManager cartManager;
    public void LoadShop()
    {
        foreach (PlayerOwnedObject indre in UIGamePlayManager.Instance.player.Ingredients)
        {
            var shopitemPrefab = Resources.Load<GameObject>("Prefabs/ShopItem");
            GameObject obj = Instantiate(shopitemPrefab, ShopContent);

            if (ResourceManager.Instance.IngredientDict.TryGetValue(indre.ID, out Ingredient result))
            {
                if (result != null)
                {
                    string name = result.RoleName;

                    Sprite icon = null;
                    if (AssetBundleManager.Instance.GetAssetBundle("nguyenlieu", out AssetBundle bundle))
                    {
                        if (bundle != null)
                        {
                            icon = bundle.LoadAsset<Sprite>(name);
                        }
                    }

                    var ui = obj.GetComponent<ShopItemUIController>();
                    ui.SetProp(icon, result.Name, MoneyFormatConvert.FormatCurrency(result.Price, "VND"));

                    var info = obj.GetComponent<ShopItemInfo>();
                    info.SetInfo(result.ID, result.Price);

                    // Gọi Init để đăng ký sự kiện click
                    ui.Init(info, this, cartManager);
                }
            }
        }
    }

    public void AddToCart(ShopItemInfo item)
    {
        cartManager.AddToCart(item);
    }
    public void OpenShop()
    {
        gameObject.SetActive(true);
    }
    public void CloseShop()
    {
        gameObject.SetActive(false);
    }
}
