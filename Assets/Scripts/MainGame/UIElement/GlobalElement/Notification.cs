using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Notification : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI content;
    public static Notification Instance;

    private Queue<(string, NotificationType)> queue = new Queue<(string, NotificationType)>();
    private bool isShowing = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Display(string text, NotificationType type)
    {
        // Thêm vào queue
        queue.Enqueue((text, type));

        // Nếu chưa hiển thị gì thì bắt đầu coroutine
        if (!isShowing)
            StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        isShowing = true;

        while (queue.Count > 0)
        {
            var (text, type) = queue.Dequeue();
            content.text = text;
            switch (type)
            {
                case NotificationType.Warning:
                    content.color = Color.yellow; break;
                case NotificationType.Normal:
                    content.color = Color.wheat; break; 
            }

            content.gameObject.SetActive(true);
            yield return DelayHelper.DisableCoroutine(content.gameObject, 4.0f);
            yield return new WaitForSeconds(0.2f);
        }

        isShowing = false;
    }
}
