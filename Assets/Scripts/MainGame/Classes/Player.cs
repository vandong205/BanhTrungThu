using System.Collections.Generic;
public class Player
{
    public string PlayerName { get; set; }
    public int TrustPoint { get; set; } 
    public long Capital { get; set; }
    public int Token { get; set; }
    public List<int> UnlockedCakes { get; set; } 
    public List<PlayerHoldIngredient> Ingredients { get; set; }
    public List<Order> Orders;
    public Player()
    {
        UnlockedCakes = new List<int>();
        Ingredients = new List<PlayerHoldIngredient>();
        Orders = new List<Order>();
    }
}