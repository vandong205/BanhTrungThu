using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIGamePlayManager : MonoBehaviour
{
    [SerializeField] GameObject RecipePanel;
    [SerializeField] GameObject StockPanel;
    [SerializeField] Transform RecipeContent;  
    [SerializeField] Transform StockContent;
    [SerializeField] GameObject ShopPanel;
    [SerializeField] Transform ShopContent;
    [SerializeField] GameObject StatPanel;
    [SerializeField] GameObject SettingPanel;
    [SerializeField] GameObject NotifiPanel;
    [SerializeField] Transform NotifiPanelContent;
    [SerializeField] GameObject MainUI;
    [SerializeField] GameObject HighUIBackground;
    [SerializeField] GameObject HighUI;
    [SerializeField] CookingProcessUIManager CookingProcessUIManager;
    [SerializeField] GameObject CookingPrecessObj;

    private UIGamePlayManager _instance;
    public static UIGamePlayManager Instance;
    public GameObject MessageBox;
    public GameObject BakerPortrait;
    public string ActiveTool;

    public Player player ;
    public  bool OpenAtap = false;
    bool notifiOpen = false;
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
       
        GameUISetUp();
        player = ResourceManager.Instance.player;
        SetPlayerStat(player.Capital,player.TrustPoint,player.Token);
        LoadingPlayerStat();
        LoadCakeRecipe();
        LoadShop();
        GamePlayController.Instance.OnLoadingUIDone?.Invoke();
        CookingProcessUIManager.RefreshIngrePanel();
    }
    public void OnRecipePanelOpen()
    {
        if (OpenAtap) return;
        RecipePanel.SetActive(true);
        OpenAtap = true;
    }
    public void OnNotifiPanelToggle()
    {
        if (notifiOpen)
        {
            NotifiPanel.SetActive(false);
            notifiOpen = false;
        }
        else
        {
            NotifiPanel.SetActive(true);
            notifiOpen = true;

        }
    }

    public void OnSettingPanelOpen()
    {
        if (OpenAtap) return;
        SettingPanel.SetActive(true);
        OpenAtap = true;
    }
    public void OnStockPanelOpen()
    {
        if (OpenAtap) return;
        StockPanel.SetActive(true);
        OpenAtap = true;
    }
    public void OnShopPanelOpen()
    {
        if(OpenAtap) return;
        ShopPanel.SetActive(true);
        OpenAtap = true;
    }
    public void OnClosePanel()
    {
        if (RecipePanel.activeSelf) RecipePanel.SetActive(false);
        if (StockPanel.activeSelf) StockPanel.SetActive(false);
        if(ShopPanel.activeSelf) ShopPanel.SetActive(false);
        if(SettingPanel.activeSelf) SettingPanel.SetActive( false);
        OpenAtap = false;
    }
    public void LoadingPlayerStat()
    {
        LoadStock();
        LoadOrders();
    }
    public void LoadOrders()
    {
        foreach (Order order in ResourceManager.Instance.player.Orders)
        {
            AddOrderToUI(order);
        }
    }
    private void LoadStock()
    {
        Debug.Log("Da goi load Stock");
        foreach (PlayerHoldIngredient indre in player.Ingredients)
        {
            string name = "";
            var stockprefab = Resources.Load<GameObject>("Prefabs/indreInStock");
            if (ResourceManager.Instance.IngredientDict.TryGetValue(indre.ID, out Ingredient result))
            {
                if (result != null)
                {
                    name = result.RoleName;

                }
                if (AssetBundleManager.Instance.GetAssetBundle("nguyenlieu", out AssetBundle bundle))
                {
                    if (bundle != null)
                    {
                        Sprite icon = bundle.LoadAsset<Sprite>(name);
                        if (icon != null)
                        {
                            stockprefab.GetComponent<IndreInStockController>().SetProp(icon, indre.Quantity.ToString(),result.Name);
                        }
                    }
                }
                Instantiate(stockprefab, StockContent);
            }
        }
    }
    private void LoadCakeRecipe()
    {
        Debug.Log("Đã gọi Load CakeRecipe");

        foreach (int cakeid in player.UnlockedCakes)
        {
            if (ResourceManager.Instance.CakeDict.TryGetValue(cakeid, out Cake cake))
            {
                if (cake == null) continue;
                Debug.Log("Load Thanh cong banh: " + cake.Name);
                // Load prefab recipe
                var recipePrefab = Resources.Load<GameObject>("Prefabs/RecipePrefab");
                if (recipePrefab == null)
                {
                    continue;
                }

                Sprite cakeIcon = null;
                string cakeName = cake.Name;

                // Load icon bánh từ AssetBundle "banh"
                if (AssetBundleManager.Instance.GetAssetBundle("banh", out AssetBundle cakeBundle))
                {
                    if (cakeBundle != null)
                    {
                        cakeIcon = cakeBundle.LoadAsset<Sprite>(cake.RoleName);
                    }
                }

                // Biến tạm cho 3 nguyên liệu
                Sprite[] indreIcons = new Sprite[3];
                string[] indreNames = new string[3];

                // Load nguyên liệu từ IngredientDict và AssetBundle "nguyenlieu"
                for (int i = 0; i < cake.Ingredients.Count && i < 3; i++)
                {
                    int indreId = cake.Ingredients[i];
                    if (ResourceManager.Instance.IngredientDict.TryGetValue(indreId, out Ingredient ingredient))
                    {
                        if (ingredient != null)
                        {
                            indreNames[i] = ingredient.Name;

                            if (AssetBundleManager.Instance.GetAssetBundle("nguyenlieu", out AssetBundle indreBundle))
                            {
                                if (indreBundle != null)
                                {
                                    indreIcons[i] = indreBundle.LoadAsset<Sprite>(ingredient.RoleName);
                                }
                            }
                        }
                    }
                }

                // Tạo instance recipe
                var recipeGO = Instantiate(recipePrefab, RecipeContent);

                // Gọi controller để gán dữ liệu
                var controller = recipeGO.GetComponent<RecipeUIController>();
                if (controller != null)
                {
                    controller.SetProp(
                        cakeIcon, cakeName,
                        indreIcons.Length > 0 ? indreIcons[0] : null, indreNames.Length > 0 ? indreNames[0] : "",
                        indreIcons.Length > 1 ? indreIcons[1] : null, indreNames.Length > 1 ? indreNames[1] : "",
                        indreIcons.Length > 2 ? indreIcons[2] : null, indreNames.Length > 2 ? indreNames[2] : ""
                    );
                }
                else
                {
                    Debug.LogError("Prefab CakeRecipeItem thiếu component RecipeUIController!");
                }
            }
        }
    }
    public void LoadShop()
    {
        Debug.Log("Da goi load Shop");
        foreach (PlayerHoldIngredient indre in player.Ingredients)
        {
            string name = "";
            var shopitem = Resources.Load<GameObject>("Prefabs/ShopItem");
            if (ResourceManager.Instance.IngredientDict.TryGetValue(indre.ID, out Ingredient result))
            {
                if (result != null)
                {
                    name = result.RoleName;

                }
                if (AssetBundleManager.Instance.GetAssetBundle("nguyenlieu", out AssetBundle bundle))
                {
                    if (bundle != null)
                    {
                        Sprite icon = bundle.LoadAsset<Sprite>(name);
                        if (icon != null)
                        {
                            shopitem.GetComponent<ShopItemUIController>().SetProp(icon, result.Name,result.Price.ToString());
                        }
                    }
                }
                else Debug.LogError("Khong tim thay assetBundle Nguyen lieu");
                Instantiate(shopitem, ShopContent);
            }
        }
    }
    public void SetPlayerStat(long money, int trustpoint,int token)
    {
        PlayerStatUIController statcontrol = StatPanel.GetComponent<PlayerStatUIController>();  
        statcontrol.SetMoney(money);
        statcontrol.SetTrustPoint(trustpoint);
        statcontrol.SetToken(token);
    }
    public void AddOrderToUI(Order order)
    {
        GameObject OrderPrefab = Resources.Load<GameObject>("Prefabs/Order");
        GameObject MoneyPrefab = Resources.Load<GameObject>("Prefabs/Money");
        GameObject TrustPointPrefab = Resources.Load<GameObject>("Prefabs/TrustPoint");
        GameObject TokenPrefab = Resources.Load<GameObject>("Prefabs/Token");

        // Tạo UI Order mới
        GameObject neworder = Instantiate(OrderPrefab, NotifiPanelContent);
        OrderUIController orderUIController = neworder.GetComponent<OrderUIController>();

        if (orderUIController != null)
        {
            // Set icon bánh + số lượng
            if (ResourceManager.Instance.CakeDict.TryGetValue(order.CakeID, out Cake cake))
            {
                if (AssetBundleManager.Instance.GetAssetBundle("banh", out AssetBundle cakebundle))
                {
                    Sprite cakeicon = cakebundle.LoadAsset<Sprite>(cake.RoleName);
                    orderUIController.SetProp(cakeicon, order.Number.ToString());
                }
            }

            // Add các reward (Receives)
            foreach (Receive receive in order.Receives)
            {
                switch (receive.Receivetype)
                {
                    case Receivetype.Money:
                        GameObject newmoney = Instantiate(MoneyPrefab);
                        newmoney.GetComponent<MoneyUIController>().SetMoney(receive.Amount);
                        orderUIController.AddReceiveItem(newmoney);
                        break;

                    case Receivetype.TrustPoint:
                        GameObject newtrustpoint = Instantiate(TrustPointPrefab);
                        newtrustpoint.GetComponent<TrustPointUIController>()
                                     .SetTrustPointAmount(receive.Amount.ToString());
                        orderUIController.AddReceiveItem(newtrustpoint);
                        break;

                    case Receivetype.Token:
                        GameObject newtoken = Instantiate(TokenPrefab);
                        newtoken.GetComponent<TokenUIController>()
                                .SetTokenAmount(receive.Amount.ToString());
                        orderUIController.AddReceiveItem(newtoken);
                        break;
                }
            }
        }

        NotifiPanel.GetComponent<NotifiPanelUIController>()
                   .SetNumberOfOrder(ResourceManager.Instance.player.Orders.Count.ToString());
    }
    private void GameUISetUp()
    {
        RecipePanel.SetActive(false);
        StockPanel.SetActive(false);
        ShopPanel.SetActive(false);
        NotifiPanel.SetActive(false);
        SettingPanel.SetActive(false);
        HighUI.SetActive(false);

    }
    public void SetActiveMessageBox(string text,bool active = true)
    {
        if (active) {
            SetActiveHighUI(true);
        }
        if (MessageBox == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/UIMessageBox");
            MessageBox = Instantiate(prefab, gameObject.transform);
        }
        MessageBox.SetActive(active);
        UIMessageBoxController controller = MessageBox.GetComponent<UIMessageBoxController>();
        if (controller != null)
        {
            controller.SetText(text);
        }
    }
    public void SetActiveCharacterTutor(bool active)
    {
        if (active)
        {
            SetActiveHighUI(true);
        }
        if (BakerPortrait == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/TutorCharacter");
            BakerPortrait = Instantiate(prefab, gameObject.transform);
        }
        BakerPortrait.SetActive(active);
    }
    public void SetActiveHighUiBg(bool active)
    {
        if (active)
        {
            SetActiveHighUI(true);
        }
        HighUIBackground.SetActive(active);
    }
    public void SetActiveHighUI(bool active)
    {
        HighUI.SetActive(active);
    }
    public void FanOnClick()
    {
        CookingPrecessObj.SetActive(true);
        OpenAtap = true;
        ActiveTool = "chao";
        if (ResourceManager.Instance.KitchenItemDict.TryGetValue(ActiveTool, out KitchenItem result))
        {
            CookingProcessUIManager.SetCookingToolText(result.Name, result.Use);
        }
        CookingProcessUIManager.TurnOnPanel(CookingProcessPanel.indre);
        CookingProcessUIManager.TurnOnPanel(CookingProcessPanel.cookingtool);
    }
    public void EOvenClick()
    {
        CookingPrecessObj.SetActive(true);
        OpenAtap = true;
        ActiveTool = "lonuong";
        if (ResourceManager.Instance.KitchenItemDict.TryGetValue(ActiveTool, out KitchenItem result))
        {
            CookingProcessUIManager.SetCookingToolText(result.Name, result.Use);
        }
        CookingProcessUIManager.TurnOnPanel(CookingProcessPanel.indre);
        CookingProcessUIManager.TurnOnPanel(CookingProcessPanel.cookingtool);
    }
    public void CookingToolProcessOnClose()
    {
        if (CookingPrecessObj.activeSelf) {
            CookingPrecessObj.SetActive(false);
        }
        OpenAtap = false;
    }
}

