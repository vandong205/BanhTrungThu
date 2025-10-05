using UnityEngine;
using UnityEngine.UI;

public class NewCakeUnlock : MonoBehaviour
{
    [SerializeField] Image cakeicon;
    public void SetCakeSprite(Sprite sprite)
    {
        cakeicon.sprite = sprite;
    }
}
