using System.Collections.Generic;

public class Order
{
    public int CakeID;
    public int Number;
    public List<Receive> Receives;
    public Order()
    {
        Receives = new List<Receive>();
    }
}