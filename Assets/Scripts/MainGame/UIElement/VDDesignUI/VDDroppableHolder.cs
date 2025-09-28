using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class VDDroppableHolder : MonoBehaviour, IDropHandler
{
    [SerializeField] private TextMeshProUGUI countTextUI;
    [SerializeField] private bool NotStack = true;

    private ObjectInfo mainitem = null;
    private int itemCount = 0;

    public int ItemCount => itemCount;

    private void Awake()
    {
        ObjectInfo childItem = GetComponentInChildren<ObjectInfo>();
        if (childItem != null)
        {
            mainitem = childItem;
            itemCount = 1;
        }
        UpdateCounterUI();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData == null || eventData.pointerDrag == null) return;

        VDDraggableItem dragData = eventData.pointerDrag.GetComponent<VDDraggableItem>();
        ObjectInfo iteminfo = eventData.pointerDrag.GetComponent<ObjectInfo>();
        if (dragData == null || iteminfo == null) return;

        if (mainitem == null)
        {
            GameObject go = Instantiate(iteminfo.gameObject, transform);
            mainitem = go.GetComponent<ObjectInfo>();
            itemCount = 1;
            dragData.parentAfterDrag = transform;
            Destroy(dragData.gameObject);
        }
        else if (mainitem.ID == iteminfo.ID)
        {
            // stack cùng loại
            itemCount++;
            dragData.parentAfterDrag = transform;
            Destroy(dragData.gameObject);
        }
        else if (!NotStack)
        {
            // override slot
            Destroy(mainitem.gameObject);
            mainitem = Instantiate(iteminfo.gameObject, transform).GetComponent<ObjectInfo>();
            itemCount = 1;
            dragData.parentAfterDrag = transform;
            Destroy(dragData.gameObject);
        }
        else
        {
            // trả về slot gốc
            dragData.parentAfterDrag = null;
        }

        UpdateCounterUI();
    }

    public bool RemoveOneItem()
    {
        if (itemCount > 0)
        {
            itemCount--;
            if (itemCount <= 0 && mainitem != null)
            {
                Destroy(mainitem.gameObject);
                mainitem = null;
            }
            UpdateCounterUI();
            return true;
        }
        return false;
    }

    public void AddOneItemBack(ObjectInfo itemPrefab)
    {
        if (mainitem == null)
        {
            mainitem = Instantiate(itemPrefab.gameObject, transform).GetComponent<ObjectInfo>();
            itemCount = 1;
        }
        else if (mainitem.ID == itemPrefab.ID)
        {
            itemCount++;
        }
        UpdateCounterUI();
    }

    private void UpdateCounterUI()
    {
        if (countTextUI == null) return;

        if (itemCount <= 1)
        {
            countTextUI.gameObject.SetActive(false);
        }
        else
        {
            countTextUI.gameObject.SetActive(true);
            countTextUI.text = itemCount.ToString();
        }
    }
}
