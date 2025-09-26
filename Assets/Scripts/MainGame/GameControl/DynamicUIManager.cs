using Unity.VisualScripting;
using UnityEngine;

public class DynamicUIManager : MonoBehaviour
{
    [SerializeField] DynamicUIPanelController LeftPanel;
    [SerializeField] DynamicUIPanelController RightPanel;
    public Billbox billbox;
  
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
    public void DisplayBillBox()
    {
        billbox.DisplayBillbox(true);
        UIGamePlayManager.Instance.OpenAtap = true;
    }
    public void HideBillBox()
    {
        billbox.DisplayBillbox(false);
        UIGamePlayManager.Instance.OpenAtap = false ;
    }
    public void SetBillBox(Sprite icon, int quantity, long price, long bunustp, long bonustoken){
        billbox.SetBillBox(icon, quantity, price, bunustp, bonustoken);
    }
}
