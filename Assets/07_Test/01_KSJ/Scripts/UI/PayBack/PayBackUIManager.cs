using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//��� ��ġ UI �Ŵ���
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
    
    public List<string> companyTitle;// = "<size=120%><color=yellow>�ٱ��� ����</color></size>\n";
    
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
        
        //��� ������ ���� ������ ����
        companyThumnail.sprite = companySprite[companyLevel];
        
        companyLevelText.text = "Lv." + (companyLevel+1);
        
        companyTitleText.text = companyValues[companyLevel].Title;
        
        companyEXPText.text = (curEXP % maxEXP) + "/" + maxEXP;
    }
    public void PayBack()
    {
        //��������� �������� �ʱ�ȭ
        //
        
    }

    public void AllInfoClear()
    {
        //��� ���� �ʱ�ȭ
        //���� �ʱ�ȭ�Ͻðڽ��ϱ�? �˾�
        //�ʱ�ȭ
    }
}
