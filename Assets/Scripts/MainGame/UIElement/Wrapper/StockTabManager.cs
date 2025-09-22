using UnityEngine;

public class StockTabManager : MonoBehaviour
{
    [SerializeField] Transform StockContent;
    public void LoadStock()
    {
        foreach (Transform child in StockContent)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (PlayerOwnedObject indre in UIGamePlayManager.Instance.player.Ingredients)
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
                            stockprefab.GetComponent<IndreInStockController>().SetProp(icon, indre.Quantity.ToString(), result.Name);
                        }
                    }
                }
                Instantiate(stockprefab, StockContent);
            }
        }
    }
    public void OpenTab()
    {
        gameObject.SetActive(true);
    }
    public void CloseTab()
    {
        gameObject.SetActive(false);
    }
}
