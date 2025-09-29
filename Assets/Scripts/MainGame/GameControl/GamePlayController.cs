using System;
using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;
public class GamePlayController : MonoBehaviour
{
    public Action OnLoadingUIDone;
    public Action GotoNextIntroStep;
    private GamePlayController _instance;
    public static GamePlayController Instance;
    [SerializeField] CookingProcessController cookcontroller;
    [SerializeField] CookingProcessUIManager cookuimanager;
    [SerializeField] BinDrop _custumerdropzone;
    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineCamera _kitchenCam;
    [SerializeField] private CinemachineCamera _serviceCam;
    public int TotalIntroStep;
    public int IntroStep = 0;
    public bool onProgress;
    public bool GotOutput = false;
    public bool _isInKitchen;
    public bool _HasDoneCakeForCustumer = false;
    private Player _player;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        _isInKitchen = false;
        CameraManager.Instance.AddCamera(_kitchenCam);
        CameraManager.Instance.AddCamera(_serviceCam);
        CameraManager.Instance.SetActiveCamera(_serviceCam);
        OnLoadingUIDone += OnPlayingTutorial;
        GotoNextIntroStep += NextIntroStep;
        _custumerdropzone.OnObjectDroped+=OnServiceCakeToCustumer;
        TotalIntroStep =  ResourceManager.Instance.introDialogList.Count;
        CustumerUI.Instance.SetCustumer(2);
        _player = ResourceManager.Instance.player;
    }
    public void ExitGame() {
        Debug.Log("Exit");
        MainGame.Instance.SavePlayer();
        Application.Quit();
    }
    private void OnPlayingTutorial()
    {
        if (!ResourceManager.Instance.player.IsFirstTimeOpenGame) return;
        UIGamePlayManager.Instance.OpenAtap = true;
        UIGamePlayManager.Instance.SetActiveCharacterTutor(true);
        UIGamePlayManager.Instance.SetActiveMessageBox(ResourceManager.Instance.introDialogList[0].Text);
        IntroStep++;
        UIGamePlayManager.Instance.SetActiveHighUiBg(true);
    }
    private void NextIntroStep()
    {
        if (IntroStep >= 0 && IntroStep < TotalIntroStep) {
            UIGamePlayManager.Instance.SetActiveMessageBox(ResourceManager.Instance.introDialogList[IntroStep].Text);
            IntroStep++;
        }
        else
        {
            OnIntroStepDone();
        }
    }
    private void OnIntroStepDone()
    {
        ResourceManager.Instance.player.IsFirstTimeOpenGame = false;    
        UIGamePlayManager.Instance.SetActiveHighUI(false);
        UIGamePlayManager.Instance.OpenAtap = false;

    }
    //Giai doan dung cong cu nha bep
    public void OnCookingProcessBtnClick()
    {
        // Nếu đang chạy process thì bỏ qua
        if (onProgress) return;

        KitchenItem toolused = KitchenRoomUIManager.Instance.ActiveTool;

        // Nếu đã có output thì clear và reset tool để có thể sử dụng lại
        if (GotOutput)
        {
            if (toolused.Rolename == "lonuong")
            {
                int newcakeid = cookcontroller.GetOutputInfo().ID;
                bool cakexist = false;
                foreach(PlayerOwnedObject cake in ResourceManager.Instance.player.Cakes)
                {
                    if(cake.ID == newcakeid)
                    {
                        cake.Quantity++;
                        cakexist = true;
                        break;
                    }
                }
                if (!cakexist)
                {
                    PlayerOwnedObject newcake = new PlayerOwnedObject(newcakeid,1);
                    ResourceManager.Instance.player.Cakes.Add(newcake);
                }
                ReceptionRoomUIManager.Instance.RefreshCakeStock();
            }
            cookuimanager.SetCookingToolText(toolused.Name, toolused.Use);
            cookuimanager.ClearOutput();
            cookuimanager.ClearInput();
            cookuimanager.RefreshIngrePanel();
            cookuimanager.RefreshTempItem();
            GotOutput = false;  
            return;
        }

        // Nếu chưa có output thì bắt đầu chế biến
        cookcontroller.ProcessOutput(toolused);
        if (!cookcontroller.GetOuputState())
        {
            Debug.Log("Da huy do khong tim thay cong thuc");
            return;
        }
            float time = ResourceManager.Instance.KitchenItemDict[toolused.Rolename].UseTime;

        onProgress = true;

        cookuimanager.RunProgress(time, () =>
        {
            cookuimanager.ShowOutputInTool(cookcontroller.GetOutputInfo());
            cookuimanager.OnCookingProcessSucceed();
            onProgress = false;
            GotOutput = true;  // đánh dấu tool đang chứa output
        });
    }
    public void ChangeRoom()
    {
        if (_isInKitchen)
        {
            KitchenRoomUIManager.Instance.SetButtonPanelActive(false);
            ReceptionRoomUIManager.Instance.SetButtonPanelActive(true); 
            CameraManager.Instance.SetActiveCamera(_serviceCam);
            CustumerUI.Instance.GoingIn();
            AudioManager.Instance.PlaySFX("bellring");
            _isInKitchen = false;
            if(_HasDoneCakeForCustumer)
                StartCoroutine(ReceptionRoomUIManager.Instance.SetActiveDummyBagDelay(true, 0.5f));
        }
        else
        {
            if (_HasDoneCakeForCustumer)
                StartCoroutine(ReceptionRoomUIManager.Instance.SetActiveDummyBagDelay(false, 0f));
            KitchenRoomUIManager.Instance.SetButtonPanelActive(true);
            ReceptionRoomUIManager.Instance.SetButtonPanelActive(false);
            CameraManager.Instance.SetActiveCamera(_kitchenCam);
            CustumerUI.Instance.Goingout();
            _isInKitchen = true;
        }
        UIGamePlayManager.Instance.RegisDynamicUIPanel();
    }
    private void OnServiceCakeToCustumer()
    {
        //tru so banh da lam
        List<PlayerOwnedObject> cakes = ReceptionRoomUIManager.Instance.getWrappedCakes();
        foreach (PlayerOwnedObject obj in cakes) { 

            foreach(PlayerOwnedObject playercake in ResourceManager.Instance.player.Cakes)
            {
                if(obj==playercake)
                {
                    playercake.Quantity -= obj.Quantity;
                    break;
                }
            }
        }
        ReceptionRoomUIManager.Instance.ClearWrappedCakeList();
        ReceptionRoomUIManager.Instance.RefreshCakeStock();

        //them tien va phan thuong
    }
    
}
