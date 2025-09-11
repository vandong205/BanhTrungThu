using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUIController : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI indrename;
    [SerializeField] TextMeshProUGUI price;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetProp(Sprite itemicon,string itemname,string itemprice)
    {
        icon.sprite = itemicon;
        price.text = itemprice;
        indrename.text = itemname;  
    }
}
