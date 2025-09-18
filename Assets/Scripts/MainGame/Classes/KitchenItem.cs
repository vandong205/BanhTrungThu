using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;

public class KitchenItem
{
    public string Name { get; set; }
    public string Rolename { get; set; }
    public string Use { get; set; }
    public float UseTime { get; set; }
    public bool IsValidInput(int[] input)
    {
        if (input == null || input.Length == 0) return false;

        int min = 0, max = 0;

        switch (Rolename)
        {
            case "chao":
                min = 0; max = 99;
                break;
            case "todungvobanh":
                min = 300; max = 399;
                break;
            case "khuongo":
                min = 500; max = 599;
                break;
            case "lonuong":
                min = 400; max = 499;
                break;
            default:
                return false; 
        }

        bool hasValid = false; 

        foreach (int id in input)
        {
            if (id == 0) continue; 
            if (id < min || id > max) return false;
            hasValid = true;
        }

        return hasValid;
    }


}