using System;
using UnityEngine;

public class ReceptionRoomUIManager : MonoBehaviour
{
    [SerializeField] NotifitabManager notifitabManager;
    [SerializeField] GameObject Buttonpanel;
    [SerializeField] MaketingPanelUIController markettingUI;
    [SerializeField] CakeRackUIManager cakeRackUI;
    [SerializeField] ShopInfoUiManager shopInfoUI;
    [SerializeField] ServiceProcessUIManager serviceProcessUI;
    public Action _LoadReceptionRoom;
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
        _LoadReceptionRoom += LoadData;

    }

    private void Start()
    {
    }
    public void LoadData()
    {
        LoadCake();
        serviceProcessUI.RefreshCakeHolder();
    }
    public void LoadCake()
    {
        cakeRackUI.LoadCakes();
    }
    public void RefreshCakeStock()
    {
        cakeRackUI.ClearAll();
        cakeRackUI.LoadCakes();
    }
    public void OnMarketingPanelOpen()
    {
        if (UIGamePlayManager.Instance.OpenAtap) return;
        markettingUI.OpenTab();
        UIGamePlayManager.Instance.OpenAtap = true;
    }
    public void OnCakeRackPanelOpen()
    {
        if (UIGamePlayManager.Instance.OpenAtap) return;
        cakeRackUI.OpenTab();
        UIGamePlayManager.Instance.OpenAtap = true;
    }
    public void OnShopInfoPanelOpen()
    {
        if (UIGamePlayManager.Instance.OpenAtap) return;
        shopInfoUI.OpenTab();
        UIGamePlayManager.Instance.OpenAtap = true;
    }

    public void OnClosePanel()
    {
        if (shopInfoUI.gameObject.activeSelf) shopInfoUI.CloseTab();
        if (markettingUI.gameObject.activeSelf) markettingUI.CloseTab();
        if (cakeRackUI.gameObject.activeSelf) cakeRackUI.CloseTab();
        UIGamePlayManager.Instance.OpenAtap = false;
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
            if (UIGamePlayManager.Instance.OpenAtap) return;
            notifitabManager.OpenTab();
            UIGamePlayManager.Instance.OpenAtap = true;
            _OrderOpen = true;


        }
    }
    public void LoadOrders()
    {
        notifitabManager.LoadOrders();
    }
    public void SetButtonPanelActive(bool active)
    {
        Buttonpanel.SetActive(active);
    }
    public void CloseButtonClick()
    {
        serviceProcessUI.TurnOffPanel(ServiceProcessPanel.all);
        UIGamePlayManager.Instance.OpenAtap = false;
    }
    public void StrayClick()
    {     
        serviceProcessUI.TurnOnPanel(ServiceProcessPanel.cake);
        UIGamePlayManager.Instance.OpenAtap = true;
    }
}
