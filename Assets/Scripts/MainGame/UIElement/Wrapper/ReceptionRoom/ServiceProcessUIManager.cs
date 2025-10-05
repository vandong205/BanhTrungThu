
using System.Collections.Generic;
using System;
using UnityEngine;

public class ServiceProcessUIManager : MonoBehaviour
{
    [SerializeField] DoneCakeHolder cakeholder;
    [SerializeField] PaperBagHolder bagHolder;
    [SerializeField] GameObject CloseBtn;

    [SerializeField] GameObject cakeSlotPrefab; 

    private void Awake()
    {
        cakeholder.InitPool(15);
        bagHolder._addItemCallback += AddCallBackToBagHolder;
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
                break;
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
        if (cakeholder == null) return;

        // Xóa toàn bộ slot đang dùng (reset về pool)
        cakeholder.ClearAll();

        foreach (PlayerOwnedObject playerCake in ResourceManager.Instance.player.Cakes)
        {
            if (playerCake == null || playerCake.Quantity <= 0)
                continue;

            if (ResourceManager.Instance.CakeDict.TryGetValue(playerCake.ID, out Cake cake) && cake != null)
            {
                Transform slotTransform = cakeholder.GetSlotFromPool();
                if (slotTransform == null) continue;

                GameObject slotObj = slotTransform.gameObject;
                slotObj.name = cake.RoleName;

                ObjectInfo info = slotObj.GetComponent<ObjectInfo>();
                info.ID = cake.ID;
                info.Name = cake.Name;
                info.RoleName = cake.RoleName;
                info.Type = ObjectType.bakedcake;

                SimulateStackHolder control = slotObj.GetComponent<SimulateStackHolder>();
                if (control != null)
                {
                    control.SetItemCount(playerCake.Quantity);

                    Sprite icon = AssetBundleManager.Instance.GetSpriteFromBundle("banh", info.RoleName);
                    control.SetIcon(icon);

                    control._removeCallback = null;
                    control._removeCallback += (objInfo) =>
                    {
                        bagHolder.AddOneItem(objInfo);
                    };
                }
                else
                {
                    Debug.LogError("Prefab slot thiếu SimulateStackHolder!");
                }
            }
        }
    }

    public void AddCallBackToBagHolder()
    {
        if (bagHolder == null) return;

        Transform content = bagHolder.getContent();
        foreach (Transform child in content)
        {
            ObjectInfo info = child.GetComponent<ObjectInfo>();
            SimulateStackHolder slotcontrol = child.GetComponent<SimulateStackHolder>();
            if (slotcontrol != null && info != null)
            {
                slotcontrol._removeCallback = null;

                foreach (Transform cakeitem in cakeholder.getContent())
                {
                    ObjectInfo cakeInfo = cakeitem.GetComponent<ObjectInfo>();
                    if (cakeInfo != null && cakeInfo.ID == info.ID)
                    {
                        SimulateStackHolder cakeslot = cakeitem.GetComponent<SimulateStackHolder>();
                        if (cakeslot != null)
                        {
                            slotcontrol._removeCallback += (objInfo) =>
                            {
                                cakeslot.AddOneItem();
                            };
                        }
                        else
                        {
                            Debug.LogWarning($"[AddCallBackToBagHolder] Không tìm thấy SimulateStackHolder trong cake slot {cakeitem.name}");
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning($"[AddCallBackToBagHolder] Slot trong bag thiếu ObjectInfo hoặc SimulateStackHolder: {child.name}");
            }
        }
    }
    public List<PlayerOwnedObject> GetWrappedCakesId()
    { 
        return bagHolder.wrappedCake;
        
    }
    public void SetClickWrapCallback(Action action)
    {
        bagHolder._onClickWrap += action;
    }
    public void ClearWrappedCake()
    {
        bagHolder.ClearWrappedCake();
    }
}
