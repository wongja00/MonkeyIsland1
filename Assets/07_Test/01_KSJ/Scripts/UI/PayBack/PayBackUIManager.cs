using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//기업 가치 UI 매니저
public class PayBackUIManager : MonoBehaviour
{
    public static PayBackUIManager instance;
    
    [SerializeField]
    private Image companyThumnail;
    
    [SerializeField]
    private List<Sprite> companySprite;
    
    [SerializeField]
    private TextMeshProUGUI companyTitleText;
    
    [SerializeField]
    private TextMeshProUGUI companyLevelText;
    
    public List<string> companyTitle;// = "<size=120%><color=yellow>다국적 대기업</color></size>\n";
    
    public List<CompanyValue> companyValues;
    
    public int companyLevel = 0; 
    
    [SerializeField]
    private Image companyEXP;
    
    [SerializeField]
    private TextMeshProUGUI companyEXPText;

    private int curEXP = 0;
    
    private int maxEXP = 10;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    private void Start()
    {
        companyValues = CSVReader.GetCompanyValueData();
        
        UpdateCompanyLevelIcon(curEXP);
        
    }
    
    public void UpdateCompanyLevelIcon(int exp)
    {
        if(companyLevel >= 24)
        {
            return;
        }
        
        curEXP += exp;
        
        companyLevel = Mathf.Min(curEXP / maxEXP, 24);
        
        companyEXP.fillAmount = (float)(curEXP % maxEXP) / (maxEXP);
        
        //기업 레벨에 따른 아이콘 변경
        companyThumnail.sprite = companySprite[companyLevel];
        
        companyLevelText.text = "Lv." + (companyLevel+1);
        
        companyTitleText.text = companyValues[companyLevel].Title;
        
        companyEXPText.text = (curEXP % maxEXP) + "/" + maxEXP;
    }
    public void PayBack()
    {
        //현재까지의 진행정보 초기화
        //
        
    }

    public void AllInfoClear()
    {
        //모든 정보 초기화
        //정말 초기화하시겠습니까? 팝업
        //초기화
    }
}
