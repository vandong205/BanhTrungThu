public class PlayerOwnedObject
{
    public int ID { get; set; }
    public int Quantity { get; set; }

    public PlayerOwnedObject(int iD, int quantity)
    {
        ID = iD;
        Quantity = quantity;
    }

    // Nạp chồng toán tử ==
    public static bool operator ==(PlayerOwnedObject o1, PlayerOwnedObject o2)
    {
        // Xử lý null trước để tránh NullReferenceException
        if (ReferenceEquals(o1, o2)) return true;
        if (ReferenceEquals(o1, null) || ReferenceEquals(o2, null)) return false;

        return o1.ID == o2.ID;
    }

    // Nạp chồng toán tử !=
    public static bool operator !=(PlayerOwnedObject o1, PlayerOwnedObject o2)
    {
        return !(o1 == o2);
    }

    // Override Equals
    public override bool Equals(object obj)
    {
        if (obj is PlayerOwnedObject other)
        {
            return this == other;
        }
        return false;
    }

    // Override GetHashCode
    public override int GetHashCode()
    {
        return ID.GetHashCode(); // vì bạn so sánh bằng ID
    }
}
