using TMPro;
using UnityEngine;

public class PlayerStatUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Money;
    [SerializeField] TextMeshProUGUI TrustPoint;
    [SerializeField] TextMeshProUGUI Token;
    public void SetMoney(long money)
    {
        string result = MoneyFormatConvert.FormatCurrency(money,ResourceManager.Instance.player.FormatCurrency);
        Money.text = result;
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