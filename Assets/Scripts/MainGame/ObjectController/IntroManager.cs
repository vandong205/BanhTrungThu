using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
public class IntroManager : MonoBehaviour
{
    [SerializeField] public VideoPlayer player;
    private void Start()
    {
        player.loopPointReached += OnVideoEnded;
    }
    private void OnVideoEnded(VideoPlayer source)
    {
        Debug.Log("Video ended, loading scene...");
        SceneManager.LoadScene("LaunchLoading");
    }
}