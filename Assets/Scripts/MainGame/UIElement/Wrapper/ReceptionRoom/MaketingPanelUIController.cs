using UnityEngine;

public class MaketingPanelUIController : MonoBehaviour
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
