using UnityEngine;

public class CakeRackUIManager : MonoBehaviour
{
    [SerializeField] Transform Content;
    public void OpenTab()
    {
        gameObject.SetActive(true);
    }
    public void CloseTab()
    {
        gameObject.SetActive(false);
    }

    public void LoadCakes()
    {
        Debug.Log("da goi load cake rax");
        foreach (PlayerOwnedObject cake in ResourceManager.Instance.player.Cakes)
        {
            if(cake.Quantity<=0) continue;
            string name = "";
            
            var stockprefab = Resources.Load<GameObject>("Prefabs/indreInStock");
            if (ResourceManager.Instance.CakeDict.TryGetValue(cake.ID, out Cake result))
            {
                if (result != null)
                {
                    name = result.RoleName;

                }
                if (AssetBundleManager.Instance.GetAssetBundle("banh", out AssetBundle bundle))
                {
                    if (bundle != null)
                    {
                        Sprite icon = bundle.LoadAsset<Sprite>(name);
                        if (icon != null)
                        {
                            stockprefab.GetComponent<IndreInStockController>().SetProp(icon, cake.Quantity.ToString(), result.Name);
                        }
                    }
                }
                Instantiate(stockprefab, Content);
            }
        }
    }
    public void ClearAll()
    {
        foreach (Transform item in Content)
        {
            Destroy(item.gameObject);
        }
    }

}
