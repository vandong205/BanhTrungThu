using UnityEngine;
using UnityEngine.EventSystems;

public class VDDraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    private CanvasGroup canvasGroup;
    private Transform originParent;
    private VDDroppableHolder originSlot;
    private ObjectInfo objectInfo;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        objectInfo = GetComponent<ObjectInfo>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originParent = transform.parent;
        originSlot = originParent.GetComponent<VDDroppableHolder>();

        if (originSlot == null || objectInfo == null || originSlot.ItemCount <= 0)
        {
            eventData.pointerDrag = null;
            return;
        }

        // tạo clone để kéo
        Canvas canvas = GetComponentInParent<Canvas>();
        GameObject clone = Instantiate(gameObject, canvas.transform);
        clone.transform.SetAsLastSibling();
        clone.transform.position = eventData.position;

        VDDraggableItem cloneDrag = clone.GetComponent<VDDraggableItem>();
        cloneDrag.parentAfterDrag = null;
        cloneDrag.originParent = originParent;
        cloneDrag.originSlot = originSlot;

        cloneDrag.canvasGroup.blocksRaycasts = false;

        // ẩn object gốc để không thấy 2 object
        canvasGroup.alpha = 0f;

        eventData.pointerDrag = clone;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (parentAfterDrag == null)
        {
            // kéo thất bại → trả lại slot gốc
            originSlot?.AddOneItemBack(objectInfo);
            Destroy(gameObject); // hủy clone
        }
        else
        {
            // kéo thành công → giảm count của slot gốc
            originSlot?.RemoveOneItem();
            transform.SetParent(parentAfterDrag);
        }
    }
}
