using System.IO;
using UnityEngine;

public class Consts
{
    public static readonly string IngredientConfigKey = "IngredientConfig.json";
    public static readonly string CakeConfigKey = "CakeConfig.json";
    public static readonly string PlayerDefaultConfigKey = "PlayerConfig.json";
    public static readonly string AssetBundleConfigKey = "AssetBundleConfig.json";
    public static readonly string IntroConfigKey = "StartIntroConfig.json";
    public static readonly string CookingToolConfigKey = "CookingToolConfig.json";
    public static readonly string CakeFillingConfigKey = "CakeFillingConfig.json";
    public static readonly string ShapedCakeConfigKey = "ShapedCakeConfig.json"; 
    public static readonly string CakeCrustConfigKey = "CakeCrustConfig.json"; 
    public static readonly string RecipeConfigKey = "RecipeConfig.json";



    public static readonly string playerSavePath = Path.Combine(Application.persistentDataPath, "PlayerSave.json");



}