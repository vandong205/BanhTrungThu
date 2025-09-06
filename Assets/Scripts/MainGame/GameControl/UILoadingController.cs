using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UILoadingController : MonoBehaviour
{
    [SerializeField] private Slider LoadingUI;
    [SerializeField] private float smoothSpeed = 5f;

    private float targetValue = 0f;
    private Coroutine smoothRoutine;

    private void Start()
    {
        LoadingUI.value = 0f;

        if (MainGame.Instance != null)
            MainGame.Instance.OnLoadingProcess += SetLoadingPercent;
    }

    private void OnDisable()
    {
        if (MainGame.Instance != null)
            MainGame.Instance.OnLoadingProcess -= SetLoadingPercent;
    }

    /// <summary>
    /// Nhận % từ MainGame (0 - 100)
    /// </summary>
    public void SetLoadingPercent(float percent)
    {
        percent = Mathf.Clamp(percent, 0f, 100f);
        targetValue = percent / 100f;

        Debug.Log($"Da cap nhat UI: {targetValue}");

        // Nếu đang chạy coroutine thì dừng lại
        if (smoothRoutine != null)
            StopCoroutine(smoothRoutine);

        // Chạy coroutine mới
        smoothRoutine = StartCoroutine(SmoothFill());
    }

    private IEnumerator SmoothFill()
    {
        while (Mathf.Abs(LoadingUI.value - targetValue) > 0.001f)
        {
            LoadingUI.value = Mathf.Lerp(LoadingUI.value, targetValue, Time.deltaTime * smoothSpeed);
            yield return null;
        }

        // Snap chính xác về target khi đã gần bằng
        LoadingUI.value = targetValue;
        smoothRoutine = null;
    }
}
