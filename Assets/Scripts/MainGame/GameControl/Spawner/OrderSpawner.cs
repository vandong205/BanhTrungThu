using System.Collections.Generic;
using UnityEngine;

public class OrderSpawner
{
    public static Order SpawnRandomOrder()
    {
        Order order = new Order();

        // Lấy random CakeID từ ResourceManager
        int[] cakeIDs = ResourceManager.Instance.player.UnlockedCakes.ToArray();
        //ResourceManager.Instance.CakeDict.Keys.CopyTo(cakeIDs, 0);
        order.CakeID = cakeIDs[Random.Range(0, cakeIDs.Length)];

        // Số lượng bánh random từ 1 đến 5
        order.Number = Random.Range(1, 6);

        // Reward: Money
        if (ResourceManager.Instance.CakeDict.TryGetValue(order.CakeID, out Cake cake))
        {
            Receive moneyReward = new Receive
            {
                Receivetype = Receivetype.Money,
                Amount = cake.Price * order.Number
            };
            order.Receives.Add(moneyReward);
        }

        // Reward: TrustPoint
        Receive trustReward = new Receive
        {
            Receivetype = Receivetype.TrustPoint,
            Amount = Random.Range(3, 10)
        };
        order.Receives.Add(trustReward);

        // Reward: Token (15–20)
        Receive tokenReward = new Receive
        {
            Receivetype = Receivetype.Token,
            Amount = Random.Range(15, 21) // Random.Range min-inclusive, max-exclusive
        };
        order.Receives.Add(tokenReward);

        return order;
    }
}
