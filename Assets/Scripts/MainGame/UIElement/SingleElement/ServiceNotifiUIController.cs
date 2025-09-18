using TMPro;
using UnityEngine;

public class ServiceNotifiUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI number;
    [SerializeField] GameObject reddot;
    int numberoforder = 0;
    void Start()
    {;
        SetNumberOfOrder(ResourceManager.Instance.player.Orders.Count);
    }

    public void SetNumberOfOrder(int num)
    {
        numberoforder = num;
        if (!reddot.activeSelf) reddot.SetActive(true);
        if (num > 0)
        {
            number.text = num.ToString();
        }
    }
    
}
