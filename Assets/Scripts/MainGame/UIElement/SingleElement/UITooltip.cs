using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITooltip : MonoBehaviour
{
    public static UITooltip Instance;

    [Header("References")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI text;

    [Header("Settings")]
    [SerializeField] private Vector2 padding = new Vector2(20, 10);
    [SerializeField] private Vector2 offset = new Vector2(10, 10); // cách ra khỏi góc trên-right

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        panel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Hiển thị tooltip tại góc trên-right của UI element
    /// </summary>
    /// <param name="content">Text tooltip</param>
    /// <param name="target">RectTransform của UI element</param>
    public void Show(string content, RectTransform target)
    {
        if (target == null) return;

        text.text = content;

        // Lấy size lý tưởng của text
        Vector2 textSize = text.GetPreferredValues(content);
        background.rectTransform.sizeDelta = textSize + padding;

        // Tính vị trí góc trên-right dựa vào pivot và size
        Vector2 targetSize = target.rect.size;
        Vector2 pivot = target.pivot; // 0~1
        Vector3 targetPos = target.position; // world position của pivot

        Vector3 topRightWorld = targetPos + new Vector3(
            (1 - pivot.x) * targetSize.x,
            (1 - pivot.y) * targetSize.y,
            0);

        // Chuyển world pos sang local của panel parent
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            panel.parent as RectTransform,
            RectTransformUtility.WorldToScreenPoint(null, topRightWorld),
            null, // camera = null nếu canvas overlay
            out localPos);

        panel.anchoredPosition = localPos + offset;

        panel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
    }
}
