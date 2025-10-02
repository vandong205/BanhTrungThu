using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BinDrop : MonoBehaviour, IDropHandler
{
    public Action OnObjectDroped;
    public void OnDrop(PointerEventData eventData)
    {
        var dropped = eventData.pointerDrag;
        if (dropped != null)
        {

            OnObjectDroped?.Invoke();
        }
    }
}