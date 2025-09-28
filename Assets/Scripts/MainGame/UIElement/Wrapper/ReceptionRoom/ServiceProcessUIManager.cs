using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;

public class ServiceProcessUIManager : MonoBehaviour
{
    [SerializeField] DoneCakeHolder cakeholder;
    [SerializeField] PaperBagHolder bagHolder;
    [SerializeField] GameObject CloseBtn;
    private void Awake()
    {
        cakeholder.InitPool(15);
    }
    public void TurnOnPanel(ServiceProcessPanel panel)
    {
        switch (panel)
        {
            case ServiceProcessPanel.cake:
                cakeholder.gameObject.SetActive(true);
                break;
            case ServiceProcessPanel.paperbag:
                bagHolder.gameObject.SetActive(true);
                break ;
        }
        CloseBtn.SetActive(true);
    }
    public void TurnOffPanel(ServiceProcessPanel panel)
    {
        switch (panel)
        {
            case ServiceProcessPanel.cake:
                cakeholder.gameObject.SetActive(false);
                break;
            case ServiceProcessPanel.paperbag:
                bagHolder.gameObject.SetActive(false);
                break;
            case ServiceProcessPanel.all:
                cakeholder.gameObject.SetActive(false);
                bagHolder.gameObject.SetActive(false);

                break;
        }
        CloseBtn.SetActive(false);
    }

    public void RefreshCakeHolder()
    {
        if (cakeholder == null)
        {
            return;
        }
        foreach(PlayerOwnedObject playerCake in ResourceManager.Instance.player.Cakes)
        {
            if(ResourceManager.Instance.CakeDict.TryGetValue(playerCake.ID,out Cake cake)){
                if (cake != null)
                {
                    Transform slot = cakeholder.GetSlotFromPool();
                    ObjectInfo info = slot.GetComponent<ObjectInfo>();
                    if(info ==null) info = slot.AddComponent<ObjectInfo>();
                    info.ID = cake.ID;
                    info.Name = cake.Name;  
                    info.RoleName = cake.RoleName;
                    info.Type = ObjectType.bakedcake;
                    SimulateStackHolder control = slot.GetComponent<SimulateStackHolder>();

                    if (control != null) {
                        control.SetItemCount(playerCake.Quantity);
                        Sprite icon = AssetBundleManager.Instance.GetSpriteFromBundle("banh", info.RoleName);
                        control.SetIcon(icon);
                    }
                    else
                    {
                        Debug.LogWarning("khong co component SimulateSlot");
                    }
                }
            }
        }
    }
    
}
