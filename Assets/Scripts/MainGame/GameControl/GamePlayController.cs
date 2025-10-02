using System;
using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;
using UnityEngine.UIElements;
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
    public bool _HasOrder = false;
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
        _custumerdropzone.OnObjectDroped += OnServiceCakeToCustumer;
        ReceptionRoomUIManager.Instance.serviceProcessUI.SetClickWrapCallback(OnClickWrapCakes);
        TotalIntroStep =  ResourceManager.Instance.introDialogList.Count;
        CustumerUI.Instance.SetCustumer(2);
        _player = ResourceManager.Instance.player;
        DelayHelper.CallAfterDelay(RandomOrder, 10.0f);
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
        DelayHelper.CallAfterDelay(()=>Notification.Instance.Display("Hãy chuyển sang phòng bếp bên phải để bắt đầu làm bánh!", NotificationType.Normal), 2.0f);
        Notification.Instance.Display("Hãy bắt đầu với nấu nhân bánh bằng chảo !",NotificationType.Normal);
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
            Notification.Instance.Display($"", NotificationType.Warning);
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
            if (_HasOrder)
            {
                CustumerUI.Instance.GoingIn();
                AudioManager.Instance.PlaySFX("bellring");
            }
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
    private void OnClickWrapCakes()
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
        
        ReceptionRoomUIManager.Instance.RefreshCakeStock();
    }
    public void OnServiceCakeToCustumer()
    {
        ReceptionRoomUIManager.Instance.PlayArmEndAnimation();
        _HasDoneCakeForCustumer = false;
        _HasOrder = false;

        Order currentOrder = _player.CurrentOrder;
        List<PlayerOwnedObject> cakes = ReceptionRoomUIManager.Instance.getWrappedCakes();

        float mistakepercent = 0f;
        bool ValidCake = false;
        // kiểm tra đúng loại bánh
        foreach (PlayerOwnedObject obj in cakes)
        {
            if (obj.ID == currentOrder.CakeID)
            {
                ValidCake = true;   
                if (obj.Quantity < currentOrder.Number)
                {
                    int mistake = currentOrder.Number - obj.Quantity;
                    mistakepercent = (float)mistake / currentOrder.Number;
                }
                break;
            }
        }
        if (!ValidCake) mistakepercent = 1;
        bool hasAdd = false;
        bool hasSubtract = false;

        foreach (Receive receive in currentOrder.Receives)
        {
            switch (receive.Receivetype) {
                case Receivetype.TrustPoint:
                    long trustpointreward = 0;
                    if (mistakepercent == 1)
                    {
                        trustpointreward = -receive.Amount;
                        Notification.Instance.Display("Bạn sẽ bị trừ 100% điểm tín nhiệm nhận được nếu sai hoàn toàn đơn hàng!",NotificationType.Warning);
                    }
                    else
                    {
                        if (mistakepercent == 0)
                        {
                            trustpointreward = receive.Amount;
                            Notification.Instance.Display($"Bạn nhận được {trustpointreward} điểm tín nhiệm", NotificationType.Normal);
                        }
                        else
                        {
                            trustpointreward = -(long)(receive.Amount * mistakepercent);
                            Notification.Instance.Display($"Bạn bị trừ {trustpointreward} % điểm tín nhiệm !", NotificationType.Warning);
                        }
                    }
                    ResourceManager.Instance.AddPlayerStat(0, trustpointreward, 0);
                    break;               
                case Receivetype.Money:
                    long penalty = (long)(receive.Amount * mistakepercent);
                    long reward = receive.Amount - penalty;
                    if (mistakepercent == 1) ResourceManager.Instance.AddPlayerStat(-penalty, 0, 0);
                    else ResourceManager.Instance.AddPlayerStat(reward,0,0);
                    if (reward > 0)
                    {
                        GeneralUIMangager.Instance.SetAddMoney(reward);
                        hasAdd = true;
                    }
                    if (penalty > 0)
                    {
                        GeneralUIMangager.Instance.SetSubtractMoney(penalty);
                        hasSubtract = true;
                    }
                    break;
                case Receivetype.Token:
                    long tokenreward = 0;
                    if (mistakepercent > 0)
                    {
                        tokenreward = 0;
                        Notification.Instance.Display("Bạn không được cộng Token nếu làm sai", NotificationType.Warning);
                    }
                    if (mistakepercent == 0)
                    {
                        tokenreward = receive.Amount;
                        Notification.Instance.Display($"Bạn nhận được {tokenreward} Token", NotificationType.Normal);

                    }
                    ResourceManager.Instance.AddPlayerStat(0, 0, tokenreward);  
                    break;
            }
        }

        if (hasAdd && hasSubtract)
        {
            StartCoroutine(GeneralUIMangager.Instance.DisplayAddAndSubtractMoney());
        }
        else if (hasAdd)
        {
            GeneralUIMangager.Instance.DisplayAddMoney();
        }
        else if (hasSubtract)
        {
            GeneralUIMangager.Instance.DisplaySubtractMoney();
        }
        ReceptionRoomUIManager.Instance.ClearWrappedCakes();
        DelayHelper.CallAfterDelay(UIGamePlayManager.Instance.LoadPlayerStat, 3.0f);

        float nextOrderTime = UnityEngine.Random.Range(10.0f, 20.0f);
        DelayHelper.CallAfterDelay(RandomOrder, nextOrderTime);
    }

    public void RandomOrder()
    {
        if (_HasOrder) return;
        _player.CurrentOrder = OrderSpawner.SpawnRandomOrder();
        _HasOrder = true;
        NewOrderNotification();
    }
    public void NewOrderNotification()
    {
        if (!_HasOrder) return;
        List<int> custumer = new List<int>(ResourceManager.Instance.CustumerDict.Keys);
        int custumerId = custumer[UnityEngine.Random.Range(0, custumer.Count)];
        UIGamePlayManager.Instance.LoadOrder();
        CustumerUI.Instance.SetCustumer(custumerId);
        if(!_isInKitchen)
        {
            CustumerUI.Instance.Goingout();  
            CustumerUI.Instance.GoingIn();
        }
        AudioManager.Instance.PlaySFX("bellring");
    }
}
