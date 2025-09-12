using UnityEngine;
using UnityEngine.EventSystems;

public class DropableHolder : MonoBehaviour, IDropHandler
{
    [SerializeField] bool NotStack = false;
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount > 0)
        {
            if (NotStack) return;
        }
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
