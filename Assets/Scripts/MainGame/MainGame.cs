using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainGame : MonoBehaviour
{
    public static MainGame Instance;

    private int LaunchingGameStep = 6;
    private int CurrentLaunchingStep = 0;

    public event Action<float> OnLoadingProcess; 

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
        yield return LoadPlayer();
        
        UpdateLaunchingProcess();
        Debug.Log("Dang tai config AssetBundle");
        yield return Loader.LoadJsonConfigIntoDict<string, BuildInBundle>(Consts.AssetBundleConfigKey, ResourceManager.Instance.AssetBundleDict);
        UpdateLaunchingProcess();
        Debug.Log("Dang tai config Intro");
        yield return Loader.LoadJsonConfigIntoList<IntroDialog>(Consts.IntroConfigKey, ResourceManager.Instance.introDialogList);
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
        Debug.Log($"Da tai thong tin cua player:{ResourceManager.Instance.player.PlayerName}");
        Debug.Log($"Lan dau mo game?:{ResourceManager.Instance.player.IsFirstTimeOpenGame}");
        SceneManager.LoadScene("GamePlay");

    }
    public IEnumerator LoadPlayer(System.Action done = null)
    {
        if (File.Exists(Consts.playerSavePath))
        {
            // Load file save ngoài
            string json = File.ReadAllText(Consts.playerSavePath);
            ResourceManager.Instance.player = JsonConvert.DeserializeObject<Player>(json);
            Debug.Log("Loaded player from save file");
        }
        else
        {
            // Lấy config mặc định từ Addressables
            yield return Loader.ParseJson<Player>(Consts.PlayerDefaultConfigKey, p =>
            {
                ResourceManager.Instance.player = p;
            });

            SavePlayer(); 
            Debug.Log("Loaded default player and created save file");
        }

        done?.Invoke();
    }
    public void SavePlayer()
    {
        try
        {
            // Serialize player thành JSON
            string json = JsonConvert.SerializeObject(ResourceManager.Instance.player, Formatting.Indented);

            // Lấy thư mục cha của file
            string dir = Path.GetDirectoryName(Consts.playerSavePath);

            // Nếu thư mục chưa có thì tạo
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                Debug.Log("Created directory: " + dir);
            }

            // Ghi file
            File.WriteAllText(Consts.playerSavePath, json);
            Debug.Log("Saved player to: " + Consts.playerSavePath);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to save player: " + ex.Message);
        }
    }

}
