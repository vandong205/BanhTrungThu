using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI content;
    private Notification() { }
    public static Notification Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void Display(string text,NotificationType type)
    {
        this.content.text = text;
        switch (type)
        {
            case NotificationType.Warning:
                this.content.color = Color.yellow; break;
            case NotificationType.Normal:
                this.content.color = Color.wheat;
                break;
        }
        content.gameObject.SetActive(true);
        DelayHelper.DisableAfterDelay(content.gameObject, 4.0f);
    }
}
