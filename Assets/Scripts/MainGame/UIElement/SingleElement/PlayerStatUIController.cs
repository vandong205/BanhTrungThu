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
    public void SetTrustPoint(long trustpoint)
    {
        TrustPoint.text = trustpoint.ToString();
    }
    public void SetToken(long token)
    {
        Token.text = token.ToString();
    }
}