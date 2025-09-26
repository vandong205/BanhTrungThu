using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Materials")]
    public Material outlineMat;
    private Material normalMat;

    private SpriteRenderer sr;
    private Image img;
    private TMP_Text tmpText;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        img = GetComponent<Image>();
        tmpText = GetComponent<TMP_Text>();

        if (sr != null)
            normalMat = sr.material;
        else if (img != null)
            normalMat = img.material;
        else if (tmpText != null)
            normalMat = tmpText.fontMaterial;
    }

    // Dành cho đối tượng UI (EventSystem)
    public void OnPointerEnter(PointerEventData eventData)
    {
        ApplyOutline(true);
        Debug.Log($"Enter: {gameObject.name}");

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"Exit: {gameObject.name}");

        ApplyOutline(false);
    }
    public void OnMouseEnterObject()
    {
        if (sr != null)
            ApplyOutline(true);
    }

    public void OnMouseExitObject()
    {
        if (sr != null)
            ApplyOutline(false);
    }

    private void ApplyOutline(bool enable)
    {
        if (outlineMat == null || normalMat == null) return;

        if (sr != null)
            sr.material = enable ? outlineMat : normalMat;
        else if (img != null)
            img.material = enable ? outlineMat : normalMat;
        else if (tmpText != null)
            tmpText.fontMaterial = enable ? outlineMat : normalMat;
    }
}
