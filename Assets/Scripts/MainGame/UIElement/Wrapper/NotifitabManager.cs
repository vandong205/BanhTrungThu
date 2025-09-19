using UnityEngine;

public class NotifitabManager : MonoBehaviour
{
    [SerializeField] Transform NotifiContent;
    public void LoadOrders()
    {
        foreach (Order order in ResourceManager.Instance.player.Orders)
        {
            AddOrderToUI(order);
        }
    }
    public void AddOrderToUI(Order order)
    {
        GameObject OrderPrefab = Resources.Load<GameObject>("Prefabs/Order");
        GameObject MoneyPrefab = Resources.Load<GameObject>("Prefabs/Money");
        GameObject TrustPointPrefab = Resources.Load<GameObject>("Prefabs/TrustPoint");
        GameObject TokenPrefab = Resources.Load<GameObject>("Prefabs/Token");

        // Tạo UI Order mới
        GameObject neworder = Instantiate(OrderPrefab, NotifiContent);
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

        gameObject.GetComponent<NotifiPanelUIController>()
                   .SetNumberOfOrder(ResourceManager.Instance.player.Orders.Count.ToString());
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
