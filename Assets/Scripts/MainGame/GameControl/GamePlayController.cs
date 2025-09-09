using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    private void Start()
    {
        UIGamePlayManager.Instance.SetPlayerStat(10, 1010, 10);
    }
    public void ExitGame() {
        Debug.Log("Exit");
        Application.Quit();
    }
}
