using System.Collections.Generic;
using UnityEngine;

public class OrderSpawner
{
    public static Order SpawnRandomOrder()
    {
        Order order = new Order();

        // Lấy random CakeID từ ResourceManager
        int[] cakeIDs = new int[ResourceManager.Instance.CakeDict.Keys.Count];
        ResourceManager.Instance.CakeDict.Keys.CopyTo(cakeIDs, 0);
        order.CakeID = cakeIDs[Random.Range(0, cakeIDs.Length)];

        // Số lượng bánh random từ 1 đến 5
        order.Number = Random.Range(1, 6);

        // Sinh ra reward (Receives)
        int rewardCount = Random.Range(1, 3); // ví dụ 1–2 phần thưởng
        for (int i = 0; i < rewardCount; i++)
        {
            Receive r = new Receive();

            // Random loại reward từ enum
            r.Receivetype = (Receivetype)Random.Range(0, System.Enum.GetValues(typeof(Receivetype)).Length);

            // Sinh giá trị theo loại
            switch (r.Receivetype)
            {
                case Receivetype.Money:
                    r.Amount = Random.Range(1000, 10001); // tiền 1k–10k
                    break;

                case Receivetype.Token:
                    r.Amount = Random.Range(1, 20); // token 1–20
                    break;

                case Receivetype.TrustPoint:
                    r.Amount = 1; // item số lượng 1 (hoặc random tùy bạn)
                    break;
            }

            order.Receives.Add(r);
        }

        return order;
    }
}
