using System.Collections.Generic;
public class Player
{
    public string PlayerName { get; set; }
    public bool IsFirstTimeOpenGame { get; set; }
    public string FormatCurrency { get; set; }
    public int TrustPoint { get; set; } 
    public long Capital { get; set; }
    public int Token { get; set; }
    public List<int> UnlockedCakes { get; set; } 
    public List<PlayerOwnedObject> Ingredients { get; set; }
    public List<PlayerOwnedObject> Cakes { get; set; }
    public List<Order> Orders;
   
    public Player()
    {
        UnlockedCakes = new List<int>();
        Ingredients = new List<PlayerOwnedObject>();
        Orders = new List<Order>();
    }
}