using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    private void Awake()
    {
        
    }
    private void Start()
    {
        if (ResourceManager.Instance.player.IsFirstTimeOpenGame)
        {
            OnPlayingTutorial();
        }
    }
    public void ExitGame() {
        Debug.Log("Exit");
        Application.Quit();
    }
    private void OnPlayingTutorial()
    {
        //UIGamePlayManager.Instance.
    }
}
