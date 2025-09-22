using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Transform ShopContent;
    public void LoadShop()
    {
        foreach (PlayerOwnedObject indre in UIGamePlayManager.Instance.player.Ingredients)
        {
            string name = "";
            var shopitem = Resources.Load<GameObject>("Prefabs/ShopItem");
            if (ResourceManager.Instance.IngredientDict.TryGetValue(indre.ID, out Ingredient result))
            {
                if (result != null)
                {
                    name = result.RoleName;

                }
                if (AssetBundleManager.Instance.GetAssetBundle("nguyenlieu", out AssetBundle bundle))
                {
                    if (bundle != null)
                    {
                        Sprite icon = bundle.LoadAsset<Sprite>(name);
                        if (icon != null)
                        {
                            shopitem.GetComponent<ShopItemUIController>().SetProp(icon, result.Name, result.Price.ToString());
                        }
                    }
                }
                Instantiate(shopitem, ShopContent);
            }
        }
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
