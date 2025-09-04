using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Dictionary<int, Ingredient> IngredientDict = new Dictionary<int, Ingredient>();
    public Dictionary<int, Cake> CakeDict = new Dictionary<int, Cake>();
    public Dictionary<string, BuildInBundle> AssetBundleDict = new Dictionary<string, BuildInBundle>();
    public Player player;

    public static ResourceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
