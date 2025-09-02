using System;
using System.Collections;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public static MainGame Instance;

    private int LaunchingGameStep = 3;
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
                ResourceManager.Instance.player = player;
            }
            else
            {
                Debug.LogError("Không tải được config player");
            }
        });
        UpdateLaunchingProcess();
    }

    private void UpdateLaunchingProcess()
    {
        CurrentLaunchingStep++;
        float percent = (CurrentLaunchingStep / (float)LaunchingGameStep) * 100f;
        OnLoadingProcess?.Invoke(percent); // safe invoke
    }
}
