using UnityEngine;

public class FanBehaviour : MonoBehaviour, IInteracable
{
    public HoverEffect hoverEffect;
    [SerializeField] AudioClip sfx;
    private void Start()
    {
        if(hoverEffect == null)
        {
            gameObject.GetComponent<HoverEffect>();
        }
    }
    public void OnHoverEnter()
    {
        Debug.Log($"{name} -> OnHoverEnter");
        hoverEffect.OnMouseEnterObject();
    }

    public void OnHoverExit()
    {
        Debug.Log($"{name} -> OnHoverExit");
        hoverEffect.OnMouseExitObject();
    }

    public void OnClick()
    {
  
        Debug.Log($"{name} -> OnClick");
        UIGamePlayManager.Instance.FanOnClick();
    
    }
}