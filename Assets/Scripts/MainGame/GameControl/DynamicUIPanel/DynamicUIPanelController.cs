using UnityEngine;
using UnityEngine.EventSystems;
public class DynamicUIPanelController : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach(Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(true);
        }    
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
