using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    public Material outlineMat;   
    private Material normalMat;  
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        normalMat = sr.material;
    }

    public void OnMouseEnterObject()
    {
        Debug.Log("Dang Hover");
        sr.material = outlineMat;
    }
    public void OnMouseExitObject()
    {
        sr.material = normalMat; 
    }
    public void OnMouseClicked()
    {
        Debug.Log("Click Fan!");
    }
}
