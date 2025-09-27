using System.Collections.Generic;
using UnityEngine;

public class ObjectPool:MonoBehaviour
{
    [SerializeField] private GameObject Content;


    private List<GameObject> slotPool = new List<GameObject>();

    public void InitPool(int count,bool stackable = false)
    {
        slotPool.Clear();
        string prefablink = "";
        if (stackable) prefablink = "Prefabs/VDInventorySlot";
        else prefablink = "Prefabs/InventorySlot";
        Debug.Log($"Da goi init pool trong{name} voi link{prefablink}");
            for (int i = 0; i < count; i++)
            {
                GameObject slot = Instantiate(Resources.Load<GameObject>(prefablink), Content.transform);

                if (slot.GetComponent<DropableHolder>() == null&&!stackable)
                    slot.AddComponent<DropableHolder>().IsNotStack(true);

                slot.SetActive(false);
                slotPool.Add(slot);
            }
    }

    public GameObject GetSlotFromPool()
    {
        foreach (var slot in slotPool)
        {
            if (!slot.activeSelf)
            {
                slot.SetActive(true);
                return slot;
            }
        }
        return null;
    }

    public void ClearAll()
    {
        foreach (var slot in slotPool)
        {
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
            slot.SetActive(false);
        }
    }
}