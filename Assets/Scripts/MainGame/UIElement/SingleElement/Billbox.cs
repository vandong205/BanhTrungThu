using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Billbox : MonoBehaviour
{
    [SerializeField] Image cakeicon;
    [SerializeField] TextMeshProUGUI _quantity;
    [SerializeField] TextMeshProUGUI _price;
    [SerializeField] TextMeshProUGUI _totalprice;
    [SerializeField] TextMeshProUGUI _bonustrustpoint;
    [SerializeField] TextMeshProUGUI _bunustoken;

    public void SetBillBox(Sprite icon,int quantity,long price, long bunustp, long bonustoken)
    {
        cakeicon.sprite = icon;
        _quantity.text = "x"+quantity;
        _price.text = MoneyFormatConvert.FormatCurrency(price,"VND");
        _bonustrustpoint.text = "x" + bunustp;
        _bunustoken.text = "x"+bonustoken;
        _totalprice.text = MoneyFormatConvert.FormatCurrency(price*quantity, "VND");
    }
    public void DisplayBillbox(bool active)
    {
        gameObject.SetActive(active);   
    }
}
