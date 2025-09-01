using System.Collections.Generic;

public class Cake
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }               
    public List<int> Ingredients { get; set; }  

    public Cake()
    {
        Ingredients = new List<int>();
    }
}
