using UnityEngine;

[System.Serializable]
public class Item
{
    public int id;           // ID duy nhất của item
    public string name;      // Tên item
    public Sprite icon;      // Icon hiển thị
    public int maxStack;     // Số lượng tối đa 1 stack
}