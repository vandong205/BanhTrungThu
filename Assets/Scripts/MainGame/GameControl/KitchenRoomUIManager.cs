using System;
using UnityEditor.Rendering;
using UnityEngine;

public class KitchenRoomUIManager : MonoBehaviour
{
    [SerializeField] CookingProcessUIManager CookingProcessUIManager;
    [SerializeField] GameObject CookingPrecessObj;
    [SerializeField] ShopManager shopmanager;
    [SerializeField] RecipeTabManager recipeTabManager;
    [SerializeField] StockTabManager stockTabManager;
 
    private KitchenRoomUIManager _instance;
    public static KitchenRoomUIManager Instance;
    public KitchenItem ActiveTool;
    public string PreTool;
    public Action _LoadKitchen;
    public Player player;
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
    void Start()
    {
        _LoadKitchen += LoadKitchenRoom;
    }
    public void LoadKitchenRoom()
    {
        player = ResourceManager.Instance.player;
        LoadingPlayerStat();
        LoadCakeRecipe();
        LoadShop();
        PreTool = "default";
        GamePlayController.Instance.OnLoadingUIDone?.Invoke();
        CookingProcessUIManager.RefreshIngrePanel();
    }
    public void OnRecipePanelOpen()
    {
        if (UIGamePlayManager.Instance.OpenAtap) return;
        recipeTabManager.OpenTab();
        UIGamePlayManager.Instance.OpenAtap = true;
    }

    public void OnStockPanelOpen()
    {
        if (UIGamePlayManager.Instance.OpenAtap) return;
        stockTabManager.OpenTab();
        UIGamePlayManager.Instance.OpenAtap = true;
    }
    public void OnShopPanelOpen()
    {
        if (UIGamePlayManager.Instance.OpenAtap) return;
        shopmanager.OpenShop();
        UIGamePlayManager.Instance.OpenAtap = true;
    }
    public void OnClosePanel()
    {
        if (recipeTabManager.gameObject.activeSelf) recipeTabManager.CloseTab();
        if (stockTabManager.gameObject.activeSelf) stockTabManager.CloseTab();
        if (shopmanager.gameObject.activeSelf) shopmanager.CloseShop();
        UIGamePlayManager.Instance.OpenAtap = false;
    }
    public void LoadingPlayerStat()
    {
        LoadStock();;
    }
    public void LoadStock()
    {
        stockTabManager.LoadStock();
    }
    private void LoadCakeRecipe()
    {
        recipeTabManager.LoadCakeRecipe();
    }
    public void LoadShop()
    {
        shopmanager.LoadShop();
    }
    public void FanOnClick()
    {
        HandleToolClick("chao", true); // chảo -> panel indre
    }

    public void EOvenClick()
    {
        HandleToolClick("lonuong"); // lò nướng -> panel tempitem
    }

    // Các tool khác cũng gọi như này:
    public void KhuongoClick()
    {
        HandleToolClick("khuongo");
    }

    public void TodungvobanhClick()
    {
        HandleToolClick("todungvobanh");
    }

    public void TodungnhanbanhClick()
    {
        HandleToolClick("todungnhanbanh");
    }

    public void GangtayClick()
    {
        HandleToolClick("gangtay");
    }

    private void HandleToolClick(string toolRoleName, bool isIndreTool = false)
    {
        CookingPrecessObj.SetActive(true);
        UIGamePlayManager.Instance.OpenAtap = true;

        if (toolRoleName != PreTool)
        {
            if (CookingProcessUIManager.HasInput()) CookingProcessUIManager.ClearInput();
            CookingProcessUIManager.RefreshIngrePanel();
            CookingProcessUIManager.RefreshTempItem();
            CookingProcessUIManager.ClearOutput();
        }

        PreTool = toolRoleName;

        if (ResourceManager.Instance.KitchenItemDict.TryGetValue(toolRoleName, out KitchenItem result))
        {
            CookingProcessUIManager.SetCookingToolText(result.Name, result.Use);
            ActiveTool = result;
        }

        // Chọn panel theo loại tool
        if (isIndreTool)
        {
            CookingProcessUIManager.TurnOnPanel(CookingProcessPanel.indre);
        }
        else
        {
            CookingProcessUIManager.TurnOnPanel(CookingProcessPanel.tempitem);
        }

        CookingProcessUIManager.TurnOnPanel(CookingProcessPanel.cookingtool);
    }

    public void CookingToolProcessOnClose()
    {
        CookingProcessUIManager.TurnOffPanel(CookingProcessPanel.all);
        UIGamePlayManager.Instance.OpenAtap = false;
    }
}
