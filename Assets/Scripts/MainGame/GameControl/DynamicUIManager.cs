using UnityEngine;

public class DynamicUIManager : MonoBehaviour
{
    [SerializeField] DynamicUIPanelController LeftPanel;
    [SerializeField] DynamicUIPanelController RightPanel;
    public void RegisPanel()
    {
        if (GamePlayController.Instance._isInKitchen)
        {

            LeftPanel.gameObject.SetActive(true);
            RightPanel.SetActiveChildUI(false);
            RightPanel.gameObject.SetActive(false);
        }
        else
        {
            LeftPanel.SetActiveChildUI(false);
            LeftPanel.gameObject.SetActive(false);
            RightPanel.gameObject.SetActive(true);
        }
    }
}
