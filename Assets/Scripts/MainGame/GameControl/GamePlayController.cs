using System;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    public Action OnLoadingUIDone;
    public Action GotoNextIntroStep;
    private GamePlayController _instance;
    public static GamePlayController Instance;
    [SerializeField] CookingProcessController cookcontroller;
    [SerializeField] CookingProcessUIManager cookuimanager;
    public int TotalIntroStep;
    public int IntroStep = 0;
    public bool onProgress;
    public bool GotOutput = false;
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
        Debug.Log("Da play tutor");

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
    }
    //Giai doan dung cong cu nha bep
    public void OnCookingProcessBtnClick()
    {
        // Nếu đang chạy process thì bỏ qua
        if (onProgress) return;

        KitchenItem toolused = UIGamePlayManager.Instance.ActiveTool;

        // Nếu đã có output thì clear và reset tool để có thể sử dụng lại
        if (GotOutput)
        {
            cookuimanager.ReturnItemToPool();
            cookuimanager.SetCookingToolText(toolused.Name, toolused.Use);

            GotOutput = false;  
            return;
        }

        // Nếu chưa có output thì bắt đầu chế biến
        cookcontroller.ProcessOutput();
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
            cookuimanager.RefreshTempItem();

            onProgress = false;
            GotOutput = true;  // đánh dấu tool đang chứa output
        });
    }



    //public bool CheckLigitRecipe()
    //{

    //}
}
