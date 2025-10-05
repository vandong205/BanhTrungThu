using UnityEngine;

public class TokenShopManager : MonoBehaviour
{
    [SerializeField] GameObject Layout;
    public void BuyNewRecipeOnClick()
    {
        if (ResourceManager.Instance.player.Token - 100 < 0)
        {
            Notification.Instance.Display("Bạn không có đủ Token",NotificationType.Warning);
            return;
        }
        UIGamePlayManager.Instance.PlayNewCakeEffect();
        ResourceManager.Instance.player.Token -= 100;
        UIGamePlayManager.Instance.LoadPlayerStat();
    }
    public void OpenTab()
    {
        Layout.SetActive(true);
        UIGamePlayManager.Instance.OpenAtap = true;
    }
    public void CloseTab()
    {
        Layout.SetActive(false);
        UIGamePlayManager.Instance.OpenAtap = false;

    }
}
