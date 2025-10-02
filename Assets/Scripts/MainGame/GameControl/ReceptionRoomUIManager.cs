
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceptionRoomUIManager : MonoBehaviour
{
    [SerializeField] GameObject Buttonpanel;
    [SerializeField] MaketingPanelUIController markettingUI;
    [SerializeField] CakeRackUIManager cakeRackUI;
    [SerializeField] ShopInfoUiManager shopInfoUI;
    [SerializeField] public ServiceProcessUIManager serviceProcessUI;
    public Action _LoadReceptionRoom;
    private ReceptionRoomUIManager _instance;
    public static ReceptionRoomUIManager Instance;
    public GameObject DummyBag;

    [SerializeField] Animator DummyBagAnimator;
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
        RefreshCakeStock();
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
        UIGamePlayManager.Instance.OpenAtap = true;
    }
    public void PaperbagClick()
    {
        serviceProcessUI.TurnOnPanel(ServiceProcessPanel.cake); 
        serviceProcessUI.TurnOnPanel(ServiceProcessPanel.paperbag);
        UIGamePlayManager.Instance.OpenAtap = true;
    }
    public IEnumerator SetActiveDummyBagDelay(bool active, float delay)
    {
        yield return new WaitForSeconds(delay);
        DummyBag.SetActive(active);
 
    }
    public List<PlayerOwnedObject> getWrappedCakes()
    {
        return serviceProcessUI.GetWrappedCakesId();
    }
    public void PlayArmEndAnimation()
    {
        DummyBagAnimator.Play("armend");
    }
    public void ClearWrappedCakes()
    {
        serviceProcessUI.ClearWrappedCake();
    }

}
