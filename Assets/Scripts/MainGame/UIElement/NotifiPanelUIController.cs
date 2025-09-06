using TMPro;
using UnityEngine;

public class NotifiPanelUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NumberOfOrder;
    public void SetNumberOfOrder(string number)
    {
        NumberOfOrder.text = number;
    }
}
