using System;
using UnityEngine;
using Unity.Cinemachine;
using Unity.VisualScripting;
public class GamePlayController : MonoBehaviour
{
    public Action OnLoadingUIDone;
    public Action GotoNextIntroStep;
    private GamePlayController _instance;
    public static GamePlayController Instance;
    [SerializeField] CookingProcessController cookcontroller;
    [SerializeField] CookingProcessUIManager cookuimanager;

    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineCamera _kitchenCam;
    [SerializeField] private CinemachineCamera _serviceCam;
    public int TotalIntroStep;
    public int IntroStep = 0;
    public bool onProgress;
    public bool GotOutput = false;
    private bool _isInKitchen;
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
        _isInKitchen = true;
        CameraManager.Instance.AddCamera(_kitchenCam);
        CameraManager.Instance.AddCamera(_serviceCam);
        CameraManager.Instance.SetActiveCamera(_kitchenCam);
        OnLoadingUIDone += OnPlayingTutorial;
        GotoNextIntroStep += NextIntroStep;
        TotalIntroStep =  ResourceManager.Instance.introDialogList.Count;
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
            CameraManager.Instance.SetActiveCamera(_serviceCam);
        }
        else
        {
            CameraManager.Instance.SetActiveCamera(_kitchenCam);

        }
    }
}
