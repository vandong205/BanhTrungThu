using UnityEngine;

public class SettingTabManager : MonoBehaviour
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
