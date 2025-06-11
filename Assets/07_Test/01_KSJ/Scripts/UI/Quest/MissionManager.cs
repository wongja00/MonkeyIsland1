using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    Banana,//�ٳ��� ��Ȯ
    MonkeyDollar,//��Ű�޷� ȹ��
    AdViewCount,//���� ��û Ƚ��
    BuildingUpgradeCount,//�ǹ���ȭ ȸ
    MonkeyConversationCount,//�����̿��� ��ȭ
    EnvironmentObservationCount,//ȯ�����
    MonkeyMailReceiveCount,//������ ����
    BananaIncentivePaymentCount,//�ٳ��� �μ�Ƽ�� ����
    GoldIncentivePaymentCount,//��� �μ�Ƽ�� ����
    MonkeyUpgradeCount,//������ ���׷��̵�
}

//�̼��� �����ϴ� Ŭ����
public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;
    
    [SerializeField]
    private MissionCard missionCard;
    
    public List<MissionData> missionList;
    
    //public int curID = 1;
    
    public 
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        missionList = CSVReader.ReadMissionCSV();
        
        int index = UnityEngine.Random.Range(1, Enum.GetValues(typeof(MissionType)).Length);

        //�̼��� �������� �ʱ�ȭ�Ѵ�.
        missionCard.Init(missionList[PayBackUIManager.instance.companyLevel * index ]);
        
        MissionTypeCheck();
    }
    
    //������ �޴´�.
    public void GetReward()
    {
        RemoveMissionTypeCheck();
        
        SkillPoint.instance.AddSkillPoint(missionCard.reward);
        
        PayBackUIManager.instance.UpdateCompanyLevelIcon(1);
        
        if (missionList.Count > 0)
        {
            //�̼��� �������� �ʱ�ȭ�Ѵ�.
            do
            {
                int index = UnityEngine.Random.Range(0, Enum.GetValues(typeof(MissionType)).Length);
                missionCard.Init(missionList[(PayBackUIManager.instance.companyLevel * 10) + index]);
            }  while ((missionCard.missionType == MissionType.BananaIncentivePaymentCount) || (missionCard.missionType == MissionType.GoldIncentivePaymentCount));
         
        }
        
        missionCard.UpdateUI();
        MissionTypeCheck();
    }
    
    private void MissionTypeCheck()
    {
        switch (missionCard.missionType)
        {
            case MissionType.Banana:
                ShopManager.OnGoldUpdate += BananaCheck;
                BananaCheck();
                break;
            case MissionType.MonkeyDollar:
                ShopManager.OnGoldUpdate += GoldCheck;
                GoldCheck();
                break;
            case MissionType.AdViewCount:
                ADManager.instance.OnAdView += AdCheck;
                AdCheck();
                break;
            case MissionType.BuildingUpgradeCount:
                ShopManager.OnBuildingPurchase += BuildingLevelCheck;
                BuildingLevelCheck(DataContainerSetID.Building, "", 0);
                break;
            case MissionType.MonkeyConversationCount:
                MonkeyEventManager.instance.OntalkEventComplete += MonkeyTalkCheck;
                MonkeyTalkCheck();
                break;
            case MissionType.EnvironmentObservationCount:
                BackGroundEventManager.instance.EnvironmentEventComplete += EnvironmentCheck;
                EnvironmentCheck();
                break;
            case MissionType.MonkeyMailReceiveCount:
                MonkeyPostEventManager.instance.PostEventComplete += MonkeyMailCheck;
                MonkeyMailCheck();
                break;
            case MissionType.BananaIncentivePaymentCount:
                //TODO: �ٳ��� �μ�Ƽ�� ���� Ƚ�� üũ
                break;
            case MissionType.GoldIncentivePaymentCount:
                //TODO: ��� �μ�Ƽ�� ���� Ƚ�� üũ
                break;
            case MissionType.MonkeyUpgradeCount:
                ShopManager.OnUnitPurchase += MonkeyLevelCheck;
                MonkeyLevelCheck(DataContainerSetID.HarvestingUnit, "", 0);
                break;
        }
    }
    
    //goalüũ ����
    public void RemoveMissionTypeCheck()
    {
        switch (missionCard.missionType)
        {
            case MissionType.Banana:
                ShopManager.OnGoldUpdate -= BananaCheck;
                break;
            case MissionType.MonkeyDollar:
                ShopManager.OnGoldUpdate -= GoldCheck;
                break;
            case MissionType.AdViewCount:
                ADManager.instance.OnAdView -= AdCheck;
                break;
            case MissionType.BuildingUpgradeCount:
                ShopManager.OnBuildingPurchase -= BuildingLevelCheck;
                break;
            case MissionType.MonkeyConversationCount:
                MonkeyEventManager.instance.OntalkEventComplete -= MonkeyTalkCheck;
                break;
            case MissionType.EnvironmentObservationCount:
                BackGroundEventManager.instance.EnvironmentEventComplete -= EnvironmentCheck;
                break;
            case MissionType.MonkeyMailReceiveCount:
                MonkeyPostEventManager.instance.PostEventComplete -= MonkeyMailCheck;
                break;
            case MissionType.BananaIncentivePaymentCount:
                break;
        };
    }

    public void GoldCheck()
    {
        missionCard.GoalCheck((int)DataManager.GetTotalPaymentCount(PaymentType.Gold));
    }
    
    public void BananaCheck()
    {
        missionCard.GoalCheck((int)DataManager.GetTotalPaymentCount(PaymentType.Banana));
    }

    public void AdCheck()
    {
        missionCard.GoalCheck(ADManager.instance.adCount);
    }
    
    public void BuildingLevelCheck(DataContainerSetID SetID, string BuildingID, int Level)
    {
        int BuildingLevel = 0;
        
        ItemSetContaner TempBuildingSetContaner = DataContainer.Instance[DataContainerSetID.Building];
        for (int i = 0; i < TempBuildingSetContaner.Count; i++)
        {
            string SetName = TempBuildingSetContaner[i].ID;
            
            BuildingLevel += DataManager.GetBuildingLevel(SetName)+1;
        }
        
        missionCard.GoalCheck(BuildingLevel);
    }
    
    public void MonkeyTalkCheck()
    {
        missionCard.GoalCheck(MonkeyEventManager.instance.EventCount);
    }
    
    public void EnvironmentCheck()
    {
        missionCard.GoalCheck(BackGroundEventManager.instance.eventCount);
    }
    
    public void MonkeyMailCheck()
    {
        missionCard.GoalCheck(MonkeyPostEventManager.instance.eventCount);
    }
    
    public void BananaIncentiveCheck()
    {
        
    } 
    
    public void GoldIncentiveCheck()
    {
        
    }
    
    public void MonkeyLevelCheck(DataContainerSetID SetID, string UnitID, int Level)
    {
        missionCard.GoalCheck(DataManager.GetHarvestLevel() + DataManager.GetCarryLevel());
    }
    
    //�̼��� �ʱ�ȭ�Ѵ�.
}
