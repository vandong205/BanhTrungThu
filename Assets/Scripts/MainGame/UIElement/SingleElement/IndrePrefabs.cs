using TMPro;
using UnityEngine;
using UnityEngine.UI; 
public class IndrePrefabs : MonoBehaviour
{
    [SerializeField] private Image image;

    public void SetIcon(Sprite icon)
    {
        image.sprite = icon;
    }
}