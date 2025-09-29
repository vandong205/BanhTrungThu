using System.Collections.Generic;
using UnityEngine;

public class DoneCakeHolder : MonoBehaviour
{
    [SerializeField] private GameObject Content;

    private List<GameObject> slotPool = new List<GameObject>();
    public List<ObjectInfo> slots = new List<ObjectInfo>();

    public void InitPool(int count)
    {
        // Xóa toàn bộ slot cũ trong Content (nếu InitPool gọi nhiều lần)
        foreach (Transform child in Content.transform)
        {
            Destroy(child.gameObject);
        }
        slotPool.Clear();

        for (int i = 0; i < count; i++)
        {
            GameObject slot = Instantiate(Resources.Load<GameObject>("Prefabs/SimulateStackHolder"), Content.transform);
            slot.SetActive(false);
            slotPool.Add(slot);
        }

        Debug.Log($"[CakeHolder] InitPool xong, tổng số slot: {slotPool.Count}");
    }

    public Transform GetSlotFromPool()
    {
        foreach (var slot in slotPool)
        {
            if (!slot.activeSelf)
            {
                slot.SetActive(true);

                // reset dữ liệu slot trước khi dùng lại
                ObjectInfo info = slot.GetComponent<ObjectInfo>();
                if (info != null)
                {
                    info.ID = 0;
                    info.Name = "";
                    info.RoleName = "";
                    info.Type = ObjectType.none;
                }

                SimulateStackHolder holder = slot.GetComponent<SimulateStackHolder>();
                if (holder != null)
                {
                    holder.SetItemCount(0);
                    holder.SetIcon(null);
                    holder._addCallback = null;
                    holder._removeCallback = null;
                }

                return slot.transform;
            }
        }
        Debug.LogWarning("[CakeHolder] Hết slot trống!");
        return null;
    }

    public void ClearAll()
    {
        foreach (var slot in slotPool)
        {
            // reset dữ liệu trước khi disable
            ObjectInfo info = slot.GetComponent<ObjectInfo>();
            if (info != null)
            {
                info.ID = 0;
                info.Name = "";
                info.RoleName = "";
                info.Type = ObjectType.none;
            }

            SimulateStackHolder holder = slot.GetComponent<SimulateStackHolder>();
            if (holder != null)
            {
                holder.SetItemCount(0);
                holder.SetIcon(null);
                holder._addCallback = null;
                holder._removeCallback = null;
            }

            slot.SetActive(false);
        }
    }

    public Transform getContent()
    {
        return Content.transform;
    }
}
