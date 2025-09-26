using System;
using System.Collections.Generic;
using System.Linq;
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

                        if (newObj.GetComponent<DraggableUI>() == null) newObj.AddComponent<DraggableUI>();
                        if (newObj.GetComponent<ObjectInfo>() == null) newObj.AddComponent<ObjectInfo>();

                        var prefab = newObj.GetComponent<IndrePrefabs>();
                        prefab.SetIcon(ico);
                        prefab.SetTooltip(ingre.Name);
                        prefab.GetComponent<ObjectInfo>().SetProp(ObjectType.ingre, ingre.ID, ingre.Name, ingre.RoleName);
                    }
                    i++;
                }
            }
        }

    }



    // Refresh item tạm thời
    public void RefreshTempItem()
    {
        TempItemHolderControl.ClearAll();

        List<ProcessedItem> tempItems = CookingProcessController.GetTempItem();
        if (tempItems == null || tempItems.Count == 0) return;

        foreach (var item in tempItems)
        {
            if (AssetBundleManager.Instance.GetAssetBundle("vatphamtamthoi", out AssetBundle bundle))
            {
                Sprite original = bundle.LoadAsset<Sprite>(item.RoleName);
                if (original == null)
                {
                    Debug.LogWarning($"[RefreshTempItem] Không tìm thấy sprite với RoleName = {item.RoleName}");
                    continue;
                }

                // Clone để tránh reference bị reuse
                Sprite ico = Instantiate(original);

                GameObject slot = TempItemHolderControl.GetSlotFromPool();
                if (slot != null)
                {
                    GameObject newObj = Instantiate(Resources.Load<GameObject>("Prefabs/IndrePrefab"), slot.transform);

                    if (newObj.GetComponent<DraggableUI>() == null) newObj.AddComponent<DraggableUI>();
                    if (newObj.GetComponent<ObjectInfo>() == null) newObj.AddComponent<ObjectInfo>();

                    var prefab = newObj.GetComponent<IndrePrefabs>();
                    prefab.SetIcon(ico);
                    prefab.SetTooltip(item.Name);
                    var objInfo = prefab.GetComponent<ObjectInfo>();
                    objInfo.SetProp(ObjectType.ingre, item.ID, item.Name, item.RoleName);

                    Debug.Log($"[RefreshTempItem] Slot {slot.name} -> Gán sprite {item.RoleName} thành công cho item ID {item.ID}");
                }
                else
                {
                    Debug.LogWarning("[RefreshTempItem] Không lấy được slot từ pool");
                }
            }
            else
            {
                Debug.LogError("[RefreshTempItem] Không load được AssetBundle: vatphamtamthoi");
            }
        }
    }


    public void SetCookingToolText(string toolname, string tooluse)
    {
        CookingToolPanelUIHandler.SetProp(toolname, tooluse);
    }

    public void ShowOutputInTool(ProcessedItem item)
    {
        //ReturnItemToPool();
        CookingToolPanelUIHandler.SliderToglle(false);

        if (item == null) return;
        string bundlename = "vatphamtamthoi";
        if (item.ID >= 100 && item.ID < 200) bundlename = "banh";
        if (AssetBundleManager.Instance.GetAssetBundle(bundlename, out AssetBundle bundle))
        {
            Sprite ico = bundle.LoadAsset<Sprite>(item.RoleName);
            Debug.Log($"Load Sprite: {item.RoleName}, result: {ico}");
            GameObject newitem = Instantiate(Resources.Load<GameObject>("Prefabs/IndrePrefab"));
            newitem.AddComponent<ObjectInfo>().SetProp(ObjectType.ingre, item.ID, item.Name, item.RoleName);
            var script = newitem.GetComponent<IndrePrefabs>();
            script.SetIcon(ico);
            script.SetTooltip(item.Name);
            CookingToolPanelUIHandler.SetOutput(newitem);
        }

        SetCookingToolText(KitchenRoomUIManager.Instance.ActiveTool.Name, "Nhận");
    }

    public void RunProgress(float time, Action action)
    {
        GamePlayController.Instance.onProgress = true;
        CookingToolPanelUIHandler.SliderToglle(true);
        CookingToolPanelUIHandler.RunProgress(time, action);
    }
    public void OnCookingProcessSucceed()
    {
        List<int> needToRemove = new List<int>();
        CookingToolPanelUIHandler.ClearUIInput();

        List<ProcessedItem> tempitem = CookingProcessController.GetTempItem();
        int[] input = CookingToolPanelUIHandler.GetInput();

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == 0) continue;

            if (input[i] < 100)
            {
                // Tìm nguyên liệu trong danh sách player
                var ingre = ResourceManager.Instance.player.Ingredients
                    .FirstOrDefault(x => x.ID == input[i]);

                if (ingre != null)
                {
                    ingre.Quantity--;
                    if (ingre.Quantity <= 0)
                        needToRemove.Add(ingre.ID);
                }
            }
            else
            {
                // Tìm item trong tempitem
                var item = tempitem.FirstOrDefault(x => x.ID == input[i]);
                if (item != null)
                {
                    tempitem.Remove(item);
                }
            }
        }

        // Xoá nguyên liệu có quantity <= 0
        foreach (int id in needToRemove)
        {
            ResourceManager.Instance.RemovePlayerIngre(id);
        }
        KitchenRoomUIManager.Instance.LoadStock();
}

    public void ClearOutput()
    {
        CookingToolPanelUIHandler.ClearOutput();
    }
    public bool HasInput()
    {
        return CookingToolPanelUIHandler.HasInput();
    }
    public void ClearInput()
    {
        CookingToolPanelUIHandler.ClearUIInput();
    }
        
}
