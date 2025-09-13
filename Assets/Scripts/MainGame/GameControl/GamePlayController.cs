using System;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    public Action OnLoadingUIDone;
    public Action GotoNextIntroStep;
    private GamePlayController _instance;
    public static GamePlayController Instance;
    [SerializeField] CookingProcessController cookcontroller;

    public int TotalIntroStep;
    public int IntroStep = 0;
    private void Awake()
    {
        
    }
    private void Start()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;    
        DontDestroyOnLoad(gameObject);
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
        string toolused = UIGamePlayManager.Instance.ActiveTool;
        switch (toolused)
        {
            case "chao":
                Debug.Log("Dang dung chao");
                cookcontroller.ShowProcessOutput();
                break;
            case "khuongo":
                break;
            case "todungvobanh":
                break;
            case "todungnhanbanh":
                break;
            case "lonuong":
                break;
            case "gangtay":
                break;
        }
    }
    //public bool CheckLigitRecipe()
    //{

    //}
}
