using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class CookingProcessUIManager : MonoBehaviour
{
    [SerializeField] GameObject IndreHolder;
    [SerializeField] GameObject CookingToolHolder;
    [SerializeField] GameObject TempItemHolder;
    [SerializeField] IndreHolder IndreHolderControl;
    [SerializeField]  tempItemHolder TempItemHolderControl;

    [SerializeField] CookingToolPanelUIHandler CookingToolPanelUIHandler;
    [SerializeField] GameObject CloseBtn;
    [SerializeField] CookingProcessController CookingProcessController;
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
            case CookingProcessPanel.tempitem:
                TempItemHolder.SetActive(true);
                break;
           
        }
        CloseBtn.SetActive(true);
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
            case CookingProcessPanel.tempitem:
                TempItemHolder.SetActive(false);
                break;
            case CookingProcessPanel.all:
                TempItemHolder.SetActive(false);
                CookingToolHolder.SetActive(false);
                IndreHolder.SetActive(false);
                break;
        }
        CloseBtn.SetActive(false);

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
                        newingre.AddComponent<DraggableObject>();
                        slot.AddComponent<DropableHolder>();
                        DropableHolder dropable = slot.GetComponent<DropableHolder>();
                        dropable.IsNotStack(true);
                        newingre.AddComponent<ObjectInfo>().SetProp(ObjectType.ingre, ingre.ID,ingre.Name,ingre.RoleName);
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
    private List<IndrePrefabs> tempitempool = new List<IndrePrefabs>();
    public void RefreshTempItem()
    {
        int i = 0;
        List<ProcessedItem> tempitem = CookingProcessController.GetTempItem();
        if (tempitem != null) {
            if (tempitem.Count == 0) return;
            foreach (ProcessedItem item in tempitem) {
                if(AssetBundleManager.Instance.GetAssetBundle("vatphamtamthoi", out AssetBundle bundle))
                {
                    Sprite ico = bundle.LoadAsset<Sprite>(item.RoleName);
                    if (i >= tempitempool.Count)
                    {
                        GameObject slot = Instantiate(Resources.Load<GameObject>("Prefabs/InventorySlot"));
                        GameObject newitem = Instantiate(Resources.Load<GameObject>("Prefabs/IndrePrefab"), slot.transform);
                        newitem.AddComponent<DraggableObject>();
                        slot.AddComponent<DropableHolder>();
                        DropableHolder dropable = slot.GetComponent<DropableHolder>();
                        dropable.IsNotStack(true);
                        newitem.AddComponent<ObjectInfo>().SetProp(ObjectType.ingre, item.ID, item.Name, item.RoleName);
                        TempItemHolderControl.AddItem(slot);
                        var script = newitem.GetComponent<IndrePrefabs>();
                        tempitempool.Add(script);
                    }

                    // Update lại icon + data cho prefab trong pool
                    tempitempool[i].SetIcon(ico);
                    tempitempool[i].gameObject.SetActive(true);

                    i++;
                }
            }
            for (int j = i; j < tempitem.Count; j++)
            {
                tempitempool[j].gameObject.SetActive(false);
            }
        }
    }
    public void ClearItemInTool()
    {
        CookingToolPanelUIHandler.ClearItem();
    }
    public void SetCookingToolText(string toolname,string tooluse)
    {
        CookingToolPanelUIHandler.SetProp(toolname, tooluse);
    }
    public void ShowOutputInTool(ProcessedItem item)
    {
        CookingToolPanelUIHandler.ClearItem();
        CookingToolPanelUIHandler.SliderToglle(false);
        if (item == null) return;
        if(AssetBundleManager.Instance.GetAssetBundle("vatphamtamthoi",out AssetBundle bundle))
        {
            Sprite ico = bundle.LoadAsset<Sprite>(item.RoleName);
            GameObject newitem = Instantiate(Resources.Load<GameObject>("Prefabs/IndrePrefab"));
            newitem.AddComponent<ObjectInfo>().SetProp(ObjectType.ingre, item.ID, item.Name, item.RoleName);
            var script = newitem.GetComponent<IndrePrefabs>();
            script.SetIcon(ico);
            CookingToolPanelUIHandler.SetOutput(newitem);

        }
        SetCookingToolText(UIGamePlayManager.Instance.ActiveTool.Name, "Nhận");
    }
    public void RunProgress(float time,Action action)
    {
        GamePlayController.Instance.onProgress = true;
        CookingToolPanelUIHandler.SliderToglle(true);
        CookingToolPanelUIHandler.RunProgress(time,action);
    }
}
