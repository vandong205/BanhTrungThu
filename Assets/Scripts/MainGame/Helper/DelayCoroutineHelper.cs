using UnityEngine;
using System.Collections;
using System;

public class DelayHelper : MonoBehaviour
{
    private static DelayHelper _instance;

    private static DelayHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("DelayHelper");
                _instance = go.AddComponent<DelayHelper>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    // Hàm delay gọi Action
    public static void CallAfterDelay(Action action, float seconds)
    {
        Instance.StartCoroutine(CallCoroutine(action, seconds));
    }

    private static IEnumerator CallCoroutine(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }

    // Hàm delay để disable object
    public static void DisableAfterDelay(GameObject target, float seconds)
    {
        Instance.StartCoroutine(DisableCoroutine(target, seconds));
    }

    public static IEnumerator DisableCoroutine(GameObject target, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (target != null)
            target.SetActive(false);
    }
}
