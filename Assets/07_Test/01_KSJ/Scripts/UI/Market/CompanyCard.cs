using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CompanyCard : MonoBehaviour
{
    public Image thumbnail;

    public string key;
    
    public CompanyType companyType;
    
    [SerializeField]
    private TextMeshProUGUI nameText;

    public string _name;
    
    [SerializeField]
    private TextMeshProUGUI percentText;
    
    public int percent;
    
    [SerializeField]
    private TextMeshProUGUI goldText;
    
    public int gold;
    
    [SerializeField]
    private TextMeshProUGUI extraAwardText;
    
    public int extraAward;

    //임시로 해놓은거(나중에 따로 데이터 컨테이너로 생성시킬수 있게 해야함)
    
    public void Init()
    {
        nameText.text = _name;
        
        MarketUIManager.PurchaseCallBack += UpdateUI;

        percent = DataManager.GetCompanyPercent(key);
        
        UpdateUI();
    }

    public void AutoSell(bool isAutoSell)
    {
        if (isAutoSell)
        {
            MarketUIManager.autoPurchaseCallBack += Sell;
        }
        else
        {
            MarketUIManager.autoPurchaseCallBack -= Sell;
        }
    }
    
    public void Sell()
    {
        MarketUIManager.instance.SellProducts(this);
    }

    public void UpdateUI()
    {
        percentText.text = percent.ToString() + "%";

        extraAwardText.text = extraAward.ToString();
        
        FinalAwardTextUpdate(MarketUIManager.instance.GetFinalAward());
    }
    
    public void FinalAwardTextUpdate(int finalAward)
    {
        goldText.text = (finalAward * (percent / 100.0f)).ToString() + "G";
    }
    
}
