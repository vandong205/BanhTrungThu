using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUIController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Image cakeicon;
    [SerializeField] Image indre1;
    [SerializeField] Image indre2;
    [SerializeField] Image indre3;
    [SerializeField] TextMeshProUGUI cakename;
    [SerializeField] TextMeshProUGUI indre1name;
    [SerializeField] TextMeshProUGUI indre2name;
    [SerializeField] TextMeshProUGUI indre3name;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetProp(Sprite cakeiconvar, string cakenamevar,Sprite indre1iconvar,string indre1namevar,Sprite indre2iconvar, string indre2namevar, Sprite indre3iconvar,string indre3namevar)
    {
        cakeicon.sprite = cakeiconvar;
        indre1.sprite = indre1iconvar;
        indre2.sprite = indre2iconvar;
        indre3.sprite = indre3iconvar;
        
        cakename.text = cakenamevar;
        indre1name.text = indre1namevar;
        indre2name.text = indre2namevar;

        indre3name.text = indre3namevar;

    }
}
