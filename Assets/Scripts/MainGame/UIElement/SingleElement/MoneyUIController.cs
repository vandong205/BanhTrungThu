using TMPro;
using UnityEngine;

public class MoneyUIController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI Amount;
    public void SetMoney(long amount)
    {
        if (Amount != null) {
            Amount.text = amount.ToString();
        }
    }
}
