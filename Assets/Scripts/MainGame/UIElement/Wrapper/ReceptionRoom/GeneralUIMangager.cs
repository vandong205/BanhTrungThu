using UnityEngine;

public class GeneralUIMangager : MonoBehaviour
{
    [SerializeField] SettingTabManager settingui;

    public void OnOpenSettingTab()
    {
        settingui.OpenTab();
        UIGamePlayManager.Instance.OpenAtap = true;

    }

    public void OnClosePanel()
    {
        if (settingui.gameObject.activeSelf) settingui.CloseTab();
        UIGamePlayManager.Instance.OpenAtap = false;
    }
}
