using UnityEngine;

public class ServiceProcessUIManager : MonoBehaviour
{
    [SerializeField] DoneCakeHolder cakeholder;
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
            case ServiceProcessPanel.all:
                cakeholder.gameObject.SetActive(false);
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

        cakeholder.ClearAll();

        foreach (var cake in ResourceManager.Instance.player.Cakes)
        {
            if (ResourceManager.Instance.CakeDict.TryGetValue(cake.ID, out Cake item))
            {

                if (AssetBundleManager.Instance.GetAssetBundle("banh", out AssetBundle bundle))
                {
                    Sprite ico = bundle.LoadAsset<Sprite>(item.RoleName);
                    GameObject slot = cakeholder.GetSlotFromPool();
                    if (slot != null)
                    {
                        // Spawn prefab mới trong slot
                        GameObject newObj = Instantiate(Resources.Load<GameObject>("Prefabs/IndrePrefab"), slot.transform);

                        if (newObj.GetComponent<DraggableObject>() == null) newObj.AddComponent<DraggableObject>();
                        if (newObj.GetComponent<ObjectInfo>() == null) newObj.AddComponent<ObjectInfo>();

                        var prefab = newObj.GetComponent<IndrePrefabs>();
                        prefab.SetIcon(ico);
                        prefab.SetTooltip(item.Name);
                        prefab.GetComponent<ObjectInfo>().SetProp(ObjectType.ingre, item.ID, item.Name, item.RoleName);
                    }
                }
            }
        }
    }
}
