using UnityEngine;
using UnityEngine.EventSystems;

public class DropableHolder : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            DraggableObject dragObj = eventData.pointerDrag.GetComponent<DraggableObject>();
            if (dragObj != null)
            {
                dragObj.parentAfterDrag = transform;
            }
        }
    }
}
