using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    Banana,//바나나 수확
    MonkeyDollar,//몽키달러 획득
    AdViewCount,//광고 시청 횟수
    BuildingUpgradeCount,//건물강화 회
    MonkeyConversationCount,//원숭이와의 대화
    EnvironmentObservationCount,//환경관찰
    MonkeyMailReceiveCount,//원숭이 우편
    BananaIncentivePaymentCount,//바나나 인센티브 지급
    GoldIncentivePaymentCount,//골드 인센티브 지급
    MonkeyUpgradeCount,//원숭이 업그레이드
}

//미션을 관리하는 클래스
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

        //미션을 랜덤으로 초기화한다.
        missionCard.Init(missionList[PayBackUIManager.instance.companyLevel * index ]);
        
        MissionTypeCheck();
    }
    
    //보상을 받는다.
    public void GetReward()
    {
        RemoveMissionTypeCheck();
        
        SkillPoint.instance.AddSkillPoint(missionCard.reward);
        
        PayBackUIManager.instance.UpdateCompanyLevelIcon(1);
        
        if (missionList.Count > 0)
        {
            //미션을 랜덤으로 초기화한다.
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
                //TODO: 바나나 인센티브 지급 횟수 체크
                break;
            case MissionType.GoldIncentivePaymentCount:
                //TODO: 골드 인센티브 지급 횟수 체크
                break;
            case MissionType.MonkeyUpgradeCount:
                ShopManager.OnUnitPurchase += MonkeyLevelCheck;
                MonkeyLevelCheck(DataContainerSetID.HarvestingUnit, "", 0);
                break;
        }
    }
    
    //goal체크 해제
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
    
    //미션을 초기화한다.
}
