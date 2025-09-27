using UnityEngine;
using UnityEngine.EventSystems;

public class DropableHolder : MonoBehaviour, IDropHandler
{
    [SerializeField] bool NotStack = false;
    public virtual void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount > 0)
        {
            if (NotStack) return;
        }
        if (eventData.pointerDrag != null)
        {
            DraggableUI dragObj = eventData.pointerDrag.GetComponent<DraggableUI>();
            if (dragObj != null)
            {
                dragObj.parentAfterDrag = transform;
            }
        }
    }
    public void IsNotStack(bool isStack) { 
        NotStack = isStack; 
    }

}
