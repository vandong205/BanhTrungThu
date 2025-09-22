
using UnityEngine;

public class UIGamePlayManager : MonoBehaviour
{
    [SerializeField] GameObject StatPanel;
    [SerializeField] GameObject MainUI;
    [SerializeField] GameObject HighUIBackground;
    [SerializeField] GameObject HighUI;
    [SerializeField] DynamicUIManager _dynamicUI;


    private UIGamePlayManager _instance;
    public static UIGamePlayManager Instance;
    public GameObject MessageBox;
    public GameObject BakerPortrait;
    public bool OpenAtap = false;
    public Player player ;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        
        player = ResourceManager.Instance.player;
        SetPlayerStat(player.Capital,player.TrustPoint,player.Token);
        KitchenRoomUIManager.Instance._LoadKitchen?.Invoke();
        ReceptionRoomUIManager.Instance._LoadReceptionRoom?.Invoke();
        _dynamicUI.RegisPanel();

        GamePlayController.Instance.OnLoadingUIDone?.Invoke();    
    }
    public void RegisDynamicUIPanel()
    {
        _dynamicUI.RegisPanel();
    }
    public void SetPlayerStat(long money, int trustpoint,int token)
    {
        PlayerStatUIController statcontrol = StatPanel.GetComponent<PlayerStatUIController>();  
        statcontrol.SetMoney(money);
        statcontrol.SetTrustPoint(trustpoint);
        statcontrol.SetToken(token);
    }
    public void SetActiveMessageBox(string text,bool active = true)
    {
        if (active) {
            SetActiveHighUI(true);
        }
        if (MessageBox == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/UIMessageBox");
            MessageBox = Instantiate(prefab, gameObject.transform);
        }
        MessageBox.SetActive(active);
        UIMessageBoxController controller = MessageBox.GetComponent<UIMessageBoxController>();
        if (controller != null)
        {
            controller.SetText(text);
        }
    }
    public void SetActiveCharacterTutor(bool active)
    {
        if (active)
        {
            SetActiveHighUI(true);
        }
        if (BakerPortrait == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/TutorCharacter");
            BakerPortrait = Instantiate(prefab, gameObject.transform);
        }
        BakerPortrait.SetActive(active);
    }
    public void SetActiveHighUiBg(bool active)
    {
        if (active)
        {
            SetActiveHighUI(true);
        }
        HighUIBackground.SetActive(active);
    }
    public void SetActiveHighUI(bool active)
    {
        HighUI.SetActive(active);
    }
 
}

