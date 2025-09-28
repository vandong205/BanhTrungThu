using System.Collections.Generic;
using UnityEngine;

public class DoneCakeHolder : MonoBehaviour
{
    [SerializeField] private GameObject Content;


    private List<GameObject> slotPool = new List<GameObject>();
    public List<ObjectInfo> slots = new List<ObjectInfo>();
    public void InitPool(int count)
    {
        slotPool.Clear();

        for (int i = 0; i < count; i++)
        {
            GameObject slot = Instantiate(Resources.Load<GameObject>("Prefabs/SimulateStackHolder"), Content.transform);
            slot.SetActive(false);
            slotPool.Add(slot);
        }

        Debug.Log($"[CakrHolder] InitPool xong, tổng số slot: {slotPool.Count}");
    }

    public Transform GetSlotFromPool()
    {
        foreach (var slot in slotPool)
        {
            if (!slot.activeSelf)
            {
                slot.SetActive(true);
                return slot.transform; 
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

}
