using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum SkillType
{
    None,
    BananaBonus,
    DollarBonus,
    BuildingEnforce,
    MonkeyDollarIncentive,
    BananaIncentive,
    MonkeyMail,
    MonkeyReport
}

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    
    private List<SkillCard> skillList = new List<SkillCard>();
    
    private List<SkillData> skillDataList = new List<SkillData>();
    
    public SkillCard skillCard;
    
    public Transform skillCardParent;

    public int carryMonkeyCostDiscount = 0;
    
    public int harvestMonkeyCostDiscount = 0;
    
    //��Ű �޷� �߰� ��Ȯ��
    public int skillMonkeyDollorperSec = 0;
    
    //�ٳ��� �߰� ��Ȯ��
    public int skillBananaperSec = 0;
    
    //���� ���η�
    public int buildingCostDiscount = 0;
    
    //������ ���� �߰� ������
    public int monkeyMailBonus = 0;
    
    //������ ���� �߰� ������
    public int monkeyReportBonus = 0;
    
    //�ٳ��� �μ�Ƽ�� �߰� ������
    public int bananaIncentiveBonus = 0;
    
    //��Ű�޷� �μ�Ƽ�� �߰� ������
    public int monkeyDollarIncentiveBonus = 0;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        skillList = new List<SkillCard>();
        skillDataList = CSVReader.ReadSkillCSV();
        
        SkillSet();
    }

    void SkillSet()
    {
        for(int i =0; i < DataContainer.Instance[DataContainerSetID.Skill].Count; i++)
        {
            ItemContaner tempContaner = DataContainer.Instance[DataContainerSetID.Skill][i];

            if (tempContaner.ID.Contains("DI") || tempContaner.ID.Contains("BI") || tempContaner.ID.Contains("MR"))
            {
                continue;
            }
            
            if (tempContaner == null)
            {
                return;
            }
            SkillCard tempSkillCard = Instantiate(skillCard, skillCardParent);

            tempSkillCard.Init(skillDataList[i*50]);
            
            skillList.Add(tempSkillCard);
        }
    }
    
    public void SkillLevelUp(SkillCard skillCard)
    {
        if(!ShopManager.UsePaymentCount(PaymentType.SkillPoint, -skillCard.skillPoint))
            return;
        
        skillCard.skillLevel++;
        
        switch (skillCard.skillType)
        {
            case SkillType.BananaBonus:
                skillBananaperSec = skillCard.skillPercent;
                BananaManager.instance.UpdateBananaPerSecond();
                break;
            case SkillType.DollarBonus:
                skillMonkeyDollorperSec = skillCard.skillPercent;
                MonkeyDollorManager.instance.UpdateMonkeyDollorPerSecond();
                break;
            case SkillType.BananaIncentive:
                bananaIncentiveBonus = skillCard.skillPercent;
                break;
            case SkillType.MonkeyDollarIncentive:
                monkeyDollarIncentiveBonus = skillCard.skillPercent;
                break;
            case SkillType.BuildingEnforce:
                buildingCostDiscount = skillCard.skillPercent/10;
                skillCard.OnSkillUpdate += BuikdingEfoceManager.instance.BuildingPriceDisCount;
                break;
            case SkillType.MonkeyMail:
                monkeyMailBonus = skillCard.skillPercent;
                break;
            case SkillType.MonkeyReport:
                monkeyReportBonus = skillCard.skillPercent;
                break;
        }
        
        string nextID = skillCard.ID.Split('_')[0] + "_" + (skillCard.skillLevel + 1);
        
        SkillData nextSkillData = skillDataList.Find(m => m.ID == nextID);
        
        if(nextSkillData != null)
        {
            skillCard.Init(nextSkillData);
        }
        
        
        skillCard.UpdateData();
    }
}
