using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIGamePlayManager : MonoBehaviour
{
    [SerializeField] GameObject RecipePanel;
    [SerializeField] GameObject StockPanel;
    [SerializeField] Transform RecipeContent;  
    [SerializeField] Transform StockContent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        LoadingPlayerStat();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnRecipePanelOpen()
    {
        RecipePanel.SetActive(true);
    }
    public void OnStockPanelOpen()
    {
        StockPanel.SetActive(true);
    }
    public void LoadingPlayerStat()
    {
        LoadStock();
    }
    private void LoadStock()
    {
        Debug.Log("Da goi load Stock");
        foreach (PlayerHoldIngredient indre in ResourceManager.Instance.player.Ingredients)
        {
            string name = "";
            var stockprefab = Resources.Load<GameObject>("Prefabs/indreInStock");
            if (ResourceManager.Instance.IngredientDict.TryGetValue(indre.ID, out Ingredient result))
            {
                if (result != null)
                {
                    name = result.RoleName;
                    Debug.Log($"Dang tai cho nguyen lieu rollname {result.RoleName}");

                }
                if (AssetBundleManager.Instance.GetAssetBundle("nguyenlieu", out AssetBundle bundle))
                {
                    if (bundle != null)
                    {
                        Sprite icon = bundle.LoadAsset<Sprite>(name);
                        Debug.Log($"Dang tai cho nguyen lieu {name}");
                        if (icon != null)
                        {
                            stockprefab.GetComponent<IndreInStockController>().SetProp(icon, indre.Quantity.ToString(),result.Name);
                        }
                    }
                }
                else Debug.LogError("Khong tim thay assetBundle Nguyen lieu");
                Instantiate(stockprefab, StockContent);
            }
        }
    }
}
