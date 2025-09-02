using System.Collections;
using UnityEngine;

public class MainGame:MonoBehaviour
{
    private static MainGame instance;
    public static MainGame Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject );
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        StartCoroutine(LoadGame());
    }
    IEnumerator  LoadGame()
    {
        Debug.Log("Dang tai Config");
        yield return LoadConfig();
    }
 IEnumerator LoadConfig()
    {
        Debug.Log("Dang tai config nguyen lieu");
        yield return Loader.LoadJsonConfigIntoDict(Consts.IngredientConfigKey, ResourceManager.Instance.IngredientDict);

        Debug.Log("Dang tai config banh");
        yield return Loader.LoadJsonConfigIntoDict(Consts.CakeConfigKey, ResourceManager.Instance.CakeDict);

        Debug.Log("Dang tai config nguoi choi");
        yield return Loader.ParseJson<Player>(Consts.PlayerDefaultConfigKey, player =>
        {
            if (player != null)
            {
                ResourceManager.Instance.player = player;
            }
            else
            {
                Debug.LogError("khong tai duoc config player");
            }
        });
    }



}
