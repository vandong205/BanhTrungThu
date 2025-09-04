using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    public static MainGame Instance;

    private int LaunchingGameStep = 5;
    private int CurrentLaunchingStep = 0;

    public event Action<float> OnLoadingProcess; // cho UI đăng ký lắng nghe

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

    private void Start()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        Debug.Log("Đang tải Config");
        yield return LoadConfig();
        yield return LoadData();
        OnLaunchingGameDone();
    }

    IEnumerator LoadConfig()
    {
        Debug.Log("Đang tải config nguyên liệu");
        yield return Loader.LoadJsonConfigIntoDict(Consts.IngredientConfigKey, ResourceManager.Instance.IngredientDict);
        UpdateLaunchingProcess();

        Debug.Log("Đang tải config bánh");
        yield return Loader.LoadJsonConfigIntoDict(Consts.CakeConfigKey, ResourceManager.Instance.CakeDict);
        UpdateLaunchingProcess();

        Debug.Log("Đang tải config người chơi");
        yield return Loader.ParseJson<Player>(Consts.PlayerDefaultConfigKey, player =>
        {
            if (player != null)
            {
                Debug.Log(player.PlayerName);
                Debug.Log(player.Ingredients.Count);
                ResourceManager.Instance.player = player;

            }
            else
            {
                Debug.LogError("Không tải được config player");
            }
        });
        UpdateLaunchingProcess();
        Debug.Log("Dang tai config AssetBundle");
        yield return Loader.LoadJsonConfigIntoDict<string, BuildInBundle>(Consts.AssetBundleConfigKey, ResourceManager.Instance.AssetBundleDict);
        UpdateLaunchingProcess();
    }
    IEnumerator LoadData()
    {
        foreach (KeyValuePair<string, BuildInBundle> bundleconfig in ResourceManager.Instance.AssetBundleDict) {
            Debug.Log($"Đang tải Asset Bundle {bundleconfig.Key}");
            yield return Loader.LoadAssetBundle(bundleconfig.Value.Path, bundleconfig.Value.Name);
        }
        UpdateLaunchingProcess();
    }
    private void UpdateLaunchingProcess()
    {
        CurrentLaunchingStep++;
        float percent = (CurrentLaunchingStep / (float)LaunchingGameStep) * 100f;
        OnLoadingProcess?.Invoke(percent); // safe invoke
    }
    private void OnLaunchingGameDone()
    {
        SceneManager.LoadScene("GamePlay");

    }
}
