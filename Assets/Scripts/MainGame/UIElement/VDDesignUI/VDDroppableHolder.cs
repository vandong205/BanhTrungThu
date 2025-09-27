using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class VDDroppableHolder : DropableHolder
{
    [Header("UI Hiển thị số lượng")]
    [SerializeField] private GameObject quantityParent;
    [SerializeField] private TextMeshProUGUI quantityText;

    private int quantity = 0;
    private int mainID = -1;

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        DraggableUI dragObj = eventData.pointerDrag.GetComponent<DraggableUI>();
        if (dragObj == null) return;

        ObjectInfo newInfo = dragObj.GetComponent<ObjectInfo>();
        if (newInfo == null) return;

        if (quantity == 0)
        {
            base.OnDrop(eventData);
            mainID = newInfo.ID;
            quantity = 1;
            UpdateQuantityUI();
        }
        else
        {
            if (newInfo.ID == mainID)
            {
                quantity++;
                UpdateQuantityUI();
                Destroy(dragObj.gameObject);

                GameObject replacement = Object.Instantiate(dragObj.gameObject, transform);
                var repCanvas = replacement.GetComponent<CanvasGroup>();
                if (repCanvas != null) repCanvas.blocksRaycasts = true;
                var repDraggable = replacement.GetComponent<DraggableUI>();
                if (repDraggable != null)
                {
                    repDraggable.parentAfterDrag = transform;
                    //repDraggable.originalParent = transform;
                }
                if (replacement.transform is RectTransform rt)
                {
                    rt.anchoredPosition = Vector2.zero;
                    rt.localScale = Vector3.one;
                }
            }
        }
    }

    private void UpdateQuantityUI()
    {
        if (quantityParent != null)
            quantityParent.SetActive(quantity > 1);

        if (quantityText != null)
            quantityText.text = quantity.ToString();
    }

    public void SetItem(ObjectInfo info, int qty)
    {
        mainID = info.ID;
        quantity = qty;
        UpdateQuantityUI();
    }
}
