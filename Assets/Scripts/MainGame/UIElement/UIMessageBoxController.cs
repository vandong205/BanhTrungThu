using TMPro;
using UnityEngine;

public class UIMessageBoxController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI message;
    public void SetText(string text)
    {
        message.text = text;
    }
}
