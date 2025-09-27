using UnityEngine;

public class ServiceProcessUIManager : MonoBehaviour
{
    [SerializeField] DoneCakeHolder cakeholder;
    [SerializeField] PaperBagHolder bagHolder;
    [SerializeField] GameObject CloseBtn;
    private void Start()
    {
        cakeholder.InitPool(20);
    }

    public void TurnOnPanel(ServiceProcessPanel panel)
    {
        switch (panel)
        {
            case ServiceProcessPanel.cake:
                cakeholder.gameObject.SetActive(true);
                break;
            case ServiceProcessPanel.paperbag:
                bagHolder.gameObject.SetActive(true);
                break ;
        }
        CloseBtn.SetActive(true);
    }
    public void TurnOffPanel(ServiceProcessPanel panel)
    {
        switch (panel)
        {
            case ServiceProcessPanel.cake:
                cakeholder.gameObject.SetActive(false);
                break;
            case ServiceProcessPanel.paperbag:
                bagHolder.gameObject.SetActive(false);
                break;
            case ServiceProcessPanel.all:
                cakeholder.gameObject.SetActive(false);
                bagHolder.gameObject.SetActive(false);

                break;
        }
        CloseBtn.SetActive(false);
    }

    public void RefreshCakeHolder()
    {
        if (cakeholder == null)
        {
            return;
        }


        foreach (var cake in ResourceManager.Instance.player.Cakes)
        {
            if (ResourceManager.Instance.CakeDict.TryGetValue(cake.ID, out Cake item))
            {

                if (AssetBundleManager.Instance.GetAssetBundle("banh", out AssetBundle bundle))
                {
                    Sprite ico = bundle.LoadAsset<Sprite>(item.RoleName);
                    Transform slot = cakeholder.GetSlotFromPool();
                    if (slot != null)
                    {
                        GameObject newObj = Instantiate(Resources.Load<GameObject>("Prefabs/IndrePrefab"), slot);

                        if (newObj.GetComponent<DraggableUI>() == null) newObj.AddComponent<DraggableUI>();
                        if (newObj.GetComponent<ObjectInfo>() == null) newObj.AddComponent<ObjectInfo>();

                        var prefab = newObj.GetComponent<IndrePrefabs>();
                        prefab.SetIcon(ico);
                        prefab.SetTooltip(item.Name);

                        var objInfo = prefab.GetComponent<ObjectInfo>();
                        objInfo.SetProp(ObjectType.ingre, item.ID, item.Name, item.RoleName);
                        cakeholder.slots.Add(objInfo);

                        //// --- Gán số lượng ---
                        //var holder = slot.GetComponent<VDDroppableHolder>();
                        //if (holder != null)
                        //{
                        //    holder.SetItem(objInfo, cake.Quantity); // cake.Quantity lấy từ PlayerOwnedObject
                        //}
                    }
                }
            }
        }
    }
}
