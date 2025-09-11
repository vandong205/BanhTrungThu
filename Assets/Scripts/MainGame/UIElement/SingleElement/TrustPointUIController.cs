using TMPro;
using UnityEngine;

public class TrustPointUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Amount;
    public void SetTrustPointAmount(string amount)
    {
        Amount.text = amount;   
    }
}
