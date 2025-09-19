using Unity.Cinemachine;
using UnityEngine;

public class CameraRegister : MonoBehaviour
{
    private CinemachineCamera _cam;

    private void Awake()
    {
        _cam = GetComponent<CinemachineCamera>();
        if (CameraManager.Instance != null)
        {
            CameraManager.Instance.AddCamera(_cam);
        }
    }

    private void OnDestroy()
    {
        if (CameraManager.Instance != null)
        {
            CameraManager.Instance.RemoveCamera(_cam);
        }
    }
}
