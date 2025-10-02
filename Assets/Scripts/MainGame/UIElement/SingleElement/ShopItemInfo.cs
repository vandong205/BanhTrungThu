using JetBrains.Annotations;
using System;
using UnityEngine;

public class ShopItemInfo : MonoBehaviour
{
    public int ID;
    public long Price;
    public void SetInfo(int ID,long Price)
    {
        this.ID = ID;
        this.Price = Price;
    }
}
