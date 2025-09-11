using UnityEngine;

public class IndreHolder : MonoBehaviour
{
    [SerializeField] GameObject Content;
    public void AddItem(GameObject go)
    {
        go.transform.SetParent(Content.transform, false);
    }
}
