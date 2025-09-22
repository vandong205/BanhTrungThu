using UnityEngine;
using UnityEngine.EventSystems;
public class DynamicUIPanelController : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetActiveChildUI(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetActiveChildUI(false);
    }
    public void SetActiveChildUI(bool active)
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(active);
        }
    }
}
