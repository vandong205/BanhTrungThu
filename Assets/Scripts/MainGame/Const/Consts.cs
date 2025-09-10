using System.IO;
using UnityEngine;

public class Consts
{
    public static readonly string IngredientConfigKey = "IngredientConfig.json";
    public static readonly string CakeConfigKey = "CakeConfig.json";
    public static readonly string PlayerDefaultConfigKey = "PlayerConfig.json";
    public static readonly string AssetBundleConfigKey = "AssetBundleConfig.json";

    public static readonly string IntroConfigKey = "StartIntroConfig.json";
    public static readonly string playerSavePath = Path.Combine(Application.persistentDataPath, "PlayerSave.json");



}