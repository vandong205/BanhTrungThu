using TMPro;
using UnityEngine;

public class TokenUIController:MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Amount;
    public void SetTokenAmount(string amount)
    {
        Amount.text = amount;
    }
}