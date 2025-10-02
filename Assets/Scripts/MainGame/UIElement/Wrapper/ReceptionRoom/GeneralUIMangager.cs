using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GeneralUIMangager : MonoBehaviour
{
    [SerializeField] SettingTabManager settingui;
    [SerializeField] TextMeshProUGUI Addmoney;
    [SerializeField] TextMeshProUGUI SubtractMoney;

    private GeneralUIMangager() { }
    public static GeneralUIMangager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void OnOpenSettingTab()
    {
        settingui.OpenTab();
        UIGamePlayManager.Instance.OpenAtap = true;

    }

    public void OnClosePanel()
    {
        if (settingui.gameObject.activeSelf) settingui.CloseTab();
        UIGamePlayManager.Instance.OpenAtap = false;
    }
    public void SetAddMoney(long money)
    {
        Addmoney.text = "+"+MoneyFormatConvert.FormatCurrency(money, "VND");
    }
    public void DisplayAddMoney()
    {
        Addmoney.gameObject.SetActive(true);
        DelayHelper.DisableAfterDelay(Addmoney.gameObject, 1.5f);
    }
    public void SetSubtractMoney(long money)
    {
        SubtractMoney.text = "-"+MoneyFormatConvert.FormatCurrency(money, "VND");
    }
   public void DisplaySubtractMoney()
    {
        SubtractMoney.gameObject.SetActive(true);
        DelayHelper.DisableAfterDelay(SubtractMoney.gameObject, 1.5f);
    }
    public IEnumerator DisplayAddAndSubtractMoney()
    {
        Addmoney.gameObject.SetActive(true);
        yield return StartCoroutine(DelayHelper.DisableCoroutine(Addmoney.gameObject, 1.5f));

        SubtractMoney.gameObject.SetActive(true);
        yield return StartCoroutine(DelayHelper.DisableCoroutine(SubtractMoney.gameObject, 1.5f));
    }




}
