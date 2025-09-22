public class PlayerOwnedObject
{
    public int ID { get; set; }
    public int Quantity { get; set; }
    public PlayerOwnedObject(int iD, int quantity)
    {
        ID = iD;
        Quantity = quantity;
    }
}