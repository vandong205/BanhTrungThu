using UnityEngine;
using System;
using System.Collections.Generic;
public class PaperBagHolder:MonoBehaviour
{
    [SerializeField] Transform Content;
    public Action _addItemCallback;
    public List<ObjectInfo> wrappedCake;
    public void WrapCakeOnClick()
    {
        if (GamePlayController.Instance._HasDoneCakeForCustumer)
        {
            Notification.Instance.Display("Hãy trả túi bánh cho khách hàng trước !",NotificationType.Warning);
            return;
        }
        foreach (Transform child in Content)
        {
            ObjectInfo childInfo = child.GetComponent<ObjectInfo>();
            if(childInfo != null)
            {
                wrappedCake.Add(childInfo);
            }
        }
        ResetHolder();
        GamePlayController.Instance._HasDoneCakeForCustumer = true;
        StartCoroutine(ReceptionRoomUIManager.Instance.SetActiveDummyBagDelay(true, 0.2f));
    }
    public bool inContent(ObjectInfo info)
    {
        foreach (Transform child in Content.transform)
        {
            ObjectInfo iteminfo =  child.GetComponent<ObjectInfo>();
            if (iteminfo == null) continue;
            if (iteminfo.ID == info.ID) return true;
        }
        return false;

    }
    public Transform getItem(ObjectInfo info)
    {
        if(!inContent(info)) return null;
        foreach (Transform child in Content.transform)
        {
            ObjectInfo iteminfo = child.GetComponent<ObjectInfo>();
            if (iteminfo == null) continue;
            if (iteminfo.ID == info.ID) return child;
        }
        return null;
    }
    public void RemoveOneItem(ObjectInfo info)
    {
        if (!inContent(info))
        {
            Debug.Log($"Khong thay item{info.RoleName} trong PaperBag");
            return;
        }
        SimulateStackHolder slotcontrol = getItem(info).GetComponent<SimulateStackHolder>();
        if(slotcontrol != null )
        {
            slotcontrol.RemoveOneItem();
        }
        else
        {
            Debug.Log("Khong thay SimulateStackHolder trong item");
        }

    }
    public void AddOneItem(ObjectInfo info)
    {
        Transform item = getItem(info);
        if (item == null)
        {
            foreach (Transform child in Content)
            {
                ObjectInfo childInfo = child.GetComponent<ObjectInfo>();
                SimulateStackHolder slotControl = child.GetComponent<SimulateStackHolder>();

                if (slotControl == null) continue;

                if (childInfo == null || childInfo.ID == 0)
                {
                    if (childInfo == null) childInfo = child.gameObject.AddComponent<ObjectInfo>();

                    childInfo.ID = info.ID;
                    childInfo.Name = info.Name;
                    childInfo.RoleName = info.RoleName;
                    childInfo.Type = info.Type;

                    Sprite icon = AssetBundleManager.Instance.GetSpriteFromBundle("banh", info.RoleName);
                    slotControl.SetIcon(icon);

                    slotControl.SetItemCount(1);
                    _addItemCallback?.Invoke();
                    return;
                }
            }
        }
        else
        {
            SimulateStackHolder slotcontrol = item.GetComponent<SimulateStackHolder>();
            if (slotcontrol != null)
            {
                slotcontrol.AddOneItem();
            }
        }
    }



    public Transform getContent()
    {
        return Content;
    }
    public void ResetHolder()
    {
        foreach(Transform child in Content)
        {
            ObjectInfo childInfo = child.GetComponent<ObjectInfo>();
            SimulateStackHolder slotControl = child.GetComponent<SimulateStackHolder>();
            if (childInfo != null && slotControl != null) {
                childInfo.ID = 0;
                slotControl.SetItemCount(0);
            }
        }
    }
}
