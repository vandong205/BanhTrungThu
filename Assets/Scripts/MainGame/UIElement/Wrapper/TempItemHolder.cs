using System.Collections.Generic;
using UnityEngine;

public class TempItemHolder : MonoBehaviour
{
    [SerializeField] private GameObject Content;


    private List<GameObject> slotPool = new List<GameObject>();

    public void InitPool(int count)
    {
        slotPool.Clear();

        for (int i = 0; i < count; i++)
        {
            GameObject slot = Instantiate(Resources.Load<GameObject>("Prefabs/InventorySlot"), Content.transform);

            if (slot.GetComponent<DropableHolder>() == null)
                slot.AddComponent<DropableHolder>().IsNotStack(true);

            slot.SetActive(false);
            slotPool.Add(slot);
        }

        Debug.Log($"[TempItemHolder] InitPool xong, tổng số slot: {slotPool.Count}");
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
        Debug.LogWarning("[TempItemHolder] Hết slot trống!");
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

    public Transform GetContent()
    {
        return Content.transform;
    }
}
