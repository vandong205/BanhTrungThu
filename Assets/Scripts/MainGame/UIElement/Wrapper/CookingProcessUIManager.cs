using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CookingProcessUIManager : MonoBehaviour
{
    [SerializeField] GameObject IndreHolder;
    [SerializeField] GameObject CookingToolHolder;
    [SerializeField] IndreHolder IndreHolderControl;
   
    public void TurnOnPanel(CookingProcessPanel panel)
    {
        switch (panel)
        {
            case CookingProcessPanel.indre:
                IndreHolder.SetActive(true);
                break;
            case CookingProcessPanel.cookingtool:
                CookingToolHolder.SetActive(true);
                break;
        }
    }
    public void TurnOffPanel(CookingProcessPanel panel)
    {
        switch (panel)
        {
            case CookingProcessPanel.indre:
                IndreHolder.SetActive(false);
                break;
            case CookingProcessPanel.cookingtool:
                CookingToolHolder.SetActive(false);
                break;
        }
    }
    private List<IndrePrefabs> pool = new List<IndrePrefabs>();

    public void RefreshIngrePanel()
    {
        int i = 0;
        foreach (var playeringre in ResourceManager.Instance.player.Ingredients)
        {
            if (ResourceManager.Instance.IngredientDict.TryGetValue(playeringre.ID, out Ingredient ingre))
            {
                if (AssetBundleManager.Instance.GetAssetBundle("nguyenlieu", out AssetBundle bundle))
                {
                    Sprite ico = bundle.LoadAsset<Sprite>(ingre.RoleName);

                    // Nếu chưa có prefab trong pool thì tạo thêm
                    if (i >= pool.Count)
                    {
                        GameObject slot = Instantiate(Resources.Load<GameObject>("Prefabs/InventorySlot"));
                        GameObject newingre = Instantiate(Resources.Load<GameObject>("Prefabs/IndrePrefab"), slot.transform);
                        Image image = newingre.GetComponentInChildren<Image>();
                        newingre.AddComponent<DraggableObject>();
                        IndreHolderControl.AddItem(slot);
                        var script = newingre.GetComponent<IndrePrefabs>();
                        pool.Add(script);
                    }

                    // Update lại icon + data cho prefab trong pool
                    pool[i].SetIcon(ico);
                    pool[i].gameObject.SetActive(true);

                    i++;
                }
            }
        }

        // Disable các prefab dư thừa
        for (int j = i; j < pool.Count; j++)
        {
            pool[j].gameObject.SetActive(false);
        }
    }

}
