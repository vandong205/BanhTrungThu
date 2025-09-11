using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndreInStockController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI LeftNumber;
    [SerializeField] TextMeshProUGUI Name;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetProp(Sprite img,string amount,string name)
    {
        icon.sprite = img;
        LeftNumber.text = amount;
        Name.text = name;
    }
}
