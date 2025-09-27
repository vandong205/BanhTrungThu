using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class CustumerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] Image Arm;
    [SerializeField] GameObject layout;
    private CustumerUI() { }
    public static CustumerUI Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
    public void GoingIn()
    {
        layout.SetActive(true);
    }
    public void Goingout()
    {
        layout.SetActive(false);
    }
    public void End()
    {

    }
    private void SetCustumerName(string text)
    {
        Name.text = text;
    }
    public void SetCustumer(int ID)
    {
        if (ResourceManager.Instance.CustumerDict.TryGetValue(ID, out Custumer custumer))
        {
            SetCustumerName(custumer.Name);
            if (AssetBundleManager.Instance.GetAssetBundle("khachhang", out AssetBundle bundle))
            {
                Sprite armsprite = AssetBundleManager.Instance.GetSpriteFromBundle("khachhang", custumer.Rolename);
                if (armsprite != null)
                    Arm.sprite = armsprite;
                else
                    Debug.LogWarning("Khong tim thay sprite " + custumer.Rolename);
            }
        }

    }
}
