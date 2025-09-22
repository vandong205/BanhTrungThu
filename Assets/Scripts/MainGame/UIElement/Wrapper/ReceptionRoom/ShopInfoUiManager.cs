using UnityEngine;

public class ShopInfoUiManager : MonoBehaviour
{
    public void OpenTab()
    {
        gameObject.SetActive(true);
    }
    public void CloseTab()
    {
        gameObject.SetActive(false);
    }
}
