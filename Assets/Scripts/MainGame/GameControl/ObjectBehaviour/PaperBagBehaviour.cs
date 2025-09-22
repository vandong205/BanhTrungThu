using UnityEngine;

public class PaperBagBehaviour : MonoBehaviour,IInteracable
{
    public HoverEffect hoverEffect;
    private void Start()
    {
        if (hoverEffect == null)
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
    }
}
