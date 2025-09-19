using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    private List<CinemachineCamera> _cameraList = new List<CinemachineCamera>();
    public static CameraManager Instance;

    private const int ACTIVE_PRIORITY = 10;
    private const int INACTIVE_PRIORITY = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddCamera(CinemachineCamera cam)
    {
        if (!_cameraList.Contains(cam))
            _cameraList.Add(cam);
    }

    public void RemoveCamera(CinemachineCamera cam)
    {
        _cameraList.Remove(cam);
    }

    public void SetActiveCamera(CinemachineCamera cam)
    {
        if (cam == null) return;

        foreach (var camera in _cameraList)
            camera.Priority = INACTIVE_PRIORITY;

        cam.Priority = ACTIVE_PRIORITY;
    }
}
