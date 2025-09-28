using UnityEngine;
using UnityEngine.EventSystems;

public class IndrePrefabs : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UnityEngine.UI.Image image;
    private string tooltipText;

    public void SetIcon(Sprite icon)
    {
        image.sprite = icon;
    }

    public void SetTooltip(string text)
    {
        if (image == null) return;
        tooltipText = text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        //if (rectTransform != null)
        //{
        //    Debug.Log("Đã gọi PointerEnter của prefabs");
        //    if (!string.IsNullOrEmpty(tooltipText))
        //        UITooltip.Instance.Show(tooltipText, rectTransform);
        //}
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        //UITooltip.Instance.Hide();
    }
}
