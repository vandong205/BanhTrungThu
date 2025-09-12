using UnityEngine;

public class IndreHolder : MonoBehaviour
{
    [SerializeField] GameObject Content;
    public void AddItem(GameObject go)
    {
        go.transform.SetParent(Content.transform, false);
    }
    public void ClearItem()
    {
        while (transform.childCount > 0)
        {
            GameObject.Destroy(transform.GetChild(0).gameObject);
        }
    }
}
