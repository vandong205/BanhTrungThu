using UnityEngine;

public class ReceptionRoomUIManager : MonoBehaviour
{
    [SerializeField] NotifitabManager notifitabManager;
    private ReceptionRoomUIManager _instance;
    public static ReceptionRoomUIManager Instance;
    private bool _OrderOpen = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }
    public void OnNotifiPanelToggle()
    {
        if (_OrderOpen)
        {
            notifitabManager.CloseTab();
            UIGamePlayManager.Instance.OpenAtap = false;
            _OrderOpen = false;

        }
        else
        {
            notifitabManager.OpenTab();
            UIGamePlayManager.Instance.OpenAtap = true;
            _OrderOpen = true;


        }
    }
    public void LoadOrders()
    {
        notifitabManager.LoadOrders();
    }
}
