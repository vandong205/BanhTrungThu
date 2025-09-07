using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    public void ExitGame() {
        Debug.Log("Exit");
        Application.Quit();
    }
}
