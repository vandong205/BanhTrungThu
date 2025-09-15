using System;
using System.Collections.Generic;
using UnityEngine;

public class CookingProcessUIManager : MonoBehaviour
{
    [SerializeField] private IndreHolder IndreHolderControl;
    [SerializeField] private TempItemHolder TempItemHolderControl;
    [SerializeField] private GameObject CookingToolHolder;
    [SerializeField] private GameObject TempItemHolder;
    [SerializeField] private CookingToolPanelUIHandler CookingToolPanelUIHandler;
    [SerializeField] private GameObject CloseBtn;
    [SerializeField] private CookingProcessController CookingProcessController;

    private void Start()
    {
        IndreHolderControl.InitPool(20);
        TempItemHolderControl.InitPool(10);
    }

    public void TurnOnPanel(CookingProcessPanel panel)
    {
        switch (panel)
        {
            case CookingProcessPanel.indre:
                IndreHolderControl.gameObject.SetActive(true);
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
                IndreHolderControl.gameObject.SetActive(false);
                break;
            case CookingProcessPanel.cookingtool:
                CookingToolHolder.SetActive(false);
                break;
            case CookingProcessPanel.tempitem:
                TempItemHolder.SetActive(false);
                break;
            case CookingProcessPanel.all:
                IndreHolderControl.gameObject.SetActive(false);
                CookingToolHolder.SetActive(false);
                TempItemHolder.SetActive(false);
                break;
        }
        CloseBtn.SetActive(false);
    }

    // Refresh nguyên liệu
    public void RefreshIngrePanel()
    {
        if (IndreHolderControl == null)
        {
            Debug.LogError("[RefreshIngrePanel] IndreHolderControl chưa được gán!");
            return;
        }

        IndreHolderControl.ClearAll();

        int i = 0;
        foreach (var playeringre in ResourceManager.Instance.player.Ingredients)
        {
            if (ResourceManager.Instance.IngredientDict.TryGetValue(playeringre.ID, out Ingredient ingre))
            {
                if (AssetBundleManager.Instance.GetAssetBundle("nguyenlieu", out AssetBundle bundle))
                {
                    Sprite ico = bundle.LoadAsset<Sprite>(ingre.RoleName);
                    GameObject slot = IndreHolderControl.GetSlotFromPool();
                    if (slot != null)
                    {
                        // Spawn prefab mới trong slot
                        GameObject newObj = Instantiate(Resources.Load<GameObject>("Prefabs/IndrePrefab"), slot.transform);

                        if (newObj.GetComponent<DraggableObject>() == null) newObj.AddComponent<DraggableObject>();
                        if (newObj.GetComponent<ObjectInfo>() == null) newObj.AddComponent<ObjectInfo>();

                        var prefab = newObj.GetComponent<IndrePrefabs>();
                        prefab.SetIcon(ico);
                        prefab.GetComponent<ObjectInfo>().SetProp(ObjectType.ingre, ingre.ID, ingre.Name, ingre.RoleName);
                    }
                    i++;
                }
            }
        }

        Debug.Log($"[RefreshIngrePanel] Tổng số ingredient hiển thị: {i}");
    }



    // Refresh item tạm thời
    public void RefreshTempItem()
    {
        TempItemHolderControl.ClearAll();

        List<ProcessedItem> tempItems = CookingProcessController.GetTempItem();
        if (tempItems == null || tempItems.Count == 0) return;

        int i = 0;
        foreach (var item in tempItems)
        {
            if (AssetBundleManager.Instance.GetAssetBundle("vatphamtamthoi", out AssetBundle bundle))
            {
                Sprite ico = bundle.LoadAsset<Sprite>(item.RoleName);
                GameObject slot = TempItemHolderControl.GetSlotFromPool();
                if (slot != null)
                {
                    var prefab = slot.GetComponentInChildren<IndrePrefabs>();
                    prefab.SetIcon(ico);
                    prefab.GetComponent<ObjectInfo>().SetProp(ObjectType.ingre, item.ID, item.Name, item.RoleName);
                }
                i++;
            }
        }
    }

    public void ReturnItemToPool()
    {
        // gọi hàm return của holder
        IndreHolderControl.ClearAll();
        RefreshIngrePanel();
    }

    public void SetCookingToolText(string toolname, string tooluse)
    {
        CookingToolPanelUIHandler.SetProp(toolname, tooluse);
    }

    public void ShowOutputInTool(ProcessedItem item)
    {
        ReturnItemToPool();
        CookingToolPanelUIHandler.SliderToglle(false);

        if (item == null) return;

        if (AssetBundleManager.Instance.GetAssetBundle("vatphamtamthoi", out AssetBundle bundle))
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

    public void RunProgress(float time, Action action)
    {
        GamePlayController.Instance.onProgress = true;
        CookingToolPanelUIHandler.SliderToglle(true);
        CookingToolPanelUIHandler.RunProgress(time, action);
    }
}
