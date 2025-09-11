using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderUIController : MonoBehaviour
{
    [SerializeField] Image cakeicon;
    [SerializeField] Transform ReceiveHolder;
    [SerializeField] TextMeshProUGUI CakeRequestNumber;
    public void SetProp(Sprite icon,string number)
    {
        cakeicon.sprite = icon;
        CakeRequestNumber.text = number;
    }
    public void AddReceiveItem(GameObject obj) {
        obj.transform.SetParent(ReceiveHolder,false);
    }
}
