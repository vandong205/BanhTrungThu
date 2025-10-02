using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUIController : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI indrename;
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] Button buyButton;

    private ShopItemInfo itemInfo;
    private ShopManager shopManager;
    private CartManager cartManager;

    public void Init(ShopItemInfo info, ShopManager manager,CartManager cart)
    {
        itemInfo = info;
        shopManager = manager;

      
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() =>
        {
            shopManager.AddToCart(itemInfo);
            cart.SetTotalMoneyUI();
            
        });
    }

    public void SetProp(Sprite itemicon, string itemname, string itemprice)
    {
        icon.sprite = itemicon;
        indrename.text = itemname;
        price.text = itemprice;
    }
}
