using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Dictionary<int, Ingredient> IngredientDict = new Dictionary<int, Ingredient>();
    public Dictionary<int, Cake> CakeDict = new Dictionary<int, Cake>();
    public Dictionary<string, BuildInBundle> AssetBundleDict = new Dictionary<string, BuildInBundle>();
    public List<IntroDialog> introDialogList = new List<IntroDialog>();
    public Dictionary<string, KitchenItem> KitchenItemDict = new Dictionary<string, KitchenItem>();
    public Dictionary<int, ProcessedItem> CakeFillingDict = new Dictionary<int, ProcessedItem>();
    public Dictionary<int, ProcessedItem> CakeCrust = new Dictionary<int, ProcessedItem>();
    public Dictionary<int, ProcessedItem> ShapedCake = new Dictionary<int, ProcessedItem>();
    public Dictionary<int, Custumer> CustumerDict = new Dictionary<int, Custumer>();

    public List<Recipe> RecipeList = new List<Recipe>();
    public RecipeBook recipeBook;
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
    public void RemovePlayerIngre(int id)
    {
        foreach(PlayerOwnedObject ingre in player.Ingredients)
        {
            if(ingre.ID == id)
            {
                player.Ingredients.Remove(ingre);
                break;
            }
        }
    }
}
