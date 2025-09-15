using System.Collections.Generic;
using UnityEngine;

public class IndreHolder : MonoBehaviour
{
    [SerializeField] private GameObject Content; // Holder chứa GridLayout
    private List<GameObject> slotPool = new List<GameObject>();

    public void InitPool(int count)
    {
        slotPool.Clear();

        for (int i = 0; i < count; i++)
        {
            // Chỉ tạo slot rỗng
            GameObject slot = Instantiate(Resources.Load<GameObject>("Prefabs/InventorySlot"), Content.transform);

            // Đảm bảo có DropableHolder
            if (slot.GetComponent<DropableHolder>() == null)
                slot.AddComponent<DropableHolder>().IsNotStack(true);

            slot.SetActive(false); // ban đầu ẩn
            slotPool.Add(slot);
        }

        Debug.Log($"[IndreHolder] InitPool xong, tổng số slot: {slotPool.Count}");
    }

    public GameObject GetSlotFromPool()
    {
        foreach (var slot in slotPool)
        {
            if (!slot.activeSelf)
            {
                slot.SetActive(true);
                Debug.Log($"[IndreHolder] GetSlotFromPool -> Trả slot {slot.name}");
                return slot;
            }
        }
        Debug.LogWarning("[IndreHolder] GetSlotFromPool -> Hết slot trống!");
        return null;
    }

    public void ClearAll()
    {
        Debug.Log("[IndreHolder] ClearAll -> Reset toàn bộ slot");
        foreach (var slot in slotPool)
        {
            // Xoá sạch prefab cũ trong slot
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }

            slot.SetActive(false);
        }
    }

    public Transform GetContent()
    {
        return Content.transform;
    }
}
