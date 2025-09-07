using TMPro;
using UnityEngine;

public class PlayerStatUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Money;
    [SerializeField] TextMeshProUGUI TrustPoint;
    [SerializeField] TextMeshProUGUI Token;
    public void SetMoney(long money)
    {
        Money.text = money.ToString();
    }
    public void SetTrustPoint(int trustpoint)
    {
        TrustPoint.text = trustpoint.ToString();
    }
    public void SetToken(int token)
    {
        Token.text = token.ToString();
    }
}