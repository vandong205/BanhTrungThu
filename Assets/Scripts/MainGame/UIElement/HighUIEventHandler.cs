using UnityEngine;
using UnityEngine.EventSystems;

public class HighUIEventHandler : MonoBehaviour,IPointerClickHandler
{
    private void Start()
    {
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("UI full screen clicked!");
        GamePlayController.Instance.GotoNextIntroStep?.Invoke();
    }
}
