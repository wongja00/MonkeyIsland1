using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class BuikdingEfoceManager : MonoBehaviour
{
    public static BuikdingEfoceManager instance;
    
    public BuildingCard buildingCard;
    
    public List<BuildingCard> buildingCardList;
    
    public List<BuildMaster> BuildingDatas;
    
    private List<BuffMaster> buffMasters = new List<BuffMaster>();

    public Transform BuildingParent;
    
    public Action OnBuildingLevelUp;
    
    public Action OnBuildingCollect;
    
    private static int levelCount = 0;
    
    public Coroutine pointCoroutine;
    
    [SerializeField]
    private List<TierUpBuilding> tierUpBuildingList = new List<TierUpBuilding>();
    
    private Dictionary<int, TierUpBuilding> tierUpBuilding = new Dictionary<int, TierUpBuilding>();
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }

        Init();
    }

    void Init()
    {       
        BuildingDatas = new List<BuildMaster>();
        
        for(int i = 0; i< DataContainer.Instance[DataContainerSetID.Building].Count; i++)
        {
            BuildMaster tempBData = CSVReader.ReadBuildMasterCSV(int.Parse(DataContainer.Instance[DataContainerSetID.Building][i].ID)); 
            BuildingDatas.Add(tempBData);
        }
        
        InitializeBuildingCards();
     
        buffMasters = CSVReader.GetBuffMaster();
    }
    
    void InitializeBuildingCards()
    {
        for (int i = 0; i < DataContainer.Instance[DataContainerSetID.Building].Count; i++)
        {
            ItemContaner tempContaner = DataContainer.Instance[DataContainerSetID.Building][i];
            if (tempContaner == null)
            {
                return;
            }
            BuildingCard tempBuildingCard = Instantiate(buildingCard, BuildingParent);
            
            tempBuildingCard.Init(BuildingDatas[i]);
            
            if(tierUpBuildingList.Count > i)
            {
                tierUpBuilding.Add(BuildingDatas[i].Build_Code, tierUpBuildingList[i]);
                if(i != 5)
                    tierUpBuildingList[i].transform.parent.gameObject.SetActive(false);
            }            
            
            //0번째는 무조건 열려있는 상태
            if (i == 0)
            {
                tempBuildingCard.openBuilding();
            }
            
            if (i > 0 && i < DataContainer.Instance[DataContainerSetID.Building].Count)
            {
                buildingCardList[i - 1].nextBuilding = tempBuildingCard;
            }
            
            buildingCardList.Add(tempBuildingCard);
        }
    }

    public void Enforce(BuildingCard card,ref int Level, double price, PaymentType paymentType)
    {
        if (ShopManager.BuildingPurchase(DataContainerSetID.Building, card.code.ToString(), price, Level, paymentType))
        {
            card.Level += 1;
        }
        
        if (card.Level > 1)
        {
            OnBuildingLevelUp?.Invoke();
        }
        else if(card.Level == 0)
        {
            if (tierUpBuilding.TryGetValue(card.code,out TierUpBuilding tempTierUpBuilding))
            {
                tempTierUpBuilding.transform.parent.gameObject.SetActive(true);
                //tempTierUpBuilding.gameObject.SetActive(true);

                if (card.BuildType == 1)
                {
                    tempTierUpBuilding.Init(card.code, PaymentType.HarvestTierUpPoint, 10, card.buildingData.Build_Value, card.buildingData.Build_Time);
                }
                else if (card.BuildType == 2)
                {
                    tempTierUpBuilding.Init(card.code, PaymentType.CarryTierUpPoint, 10, card.buildingData.Build_Value, card.buildingData.Build_Time);
                }
                else
                {
                    tempTierUpBuilding.gameObject.SetActive(false);
                }
                
                
            }
            OnBuildingCollect?.Invoke();
        }
        
        card.UpdateData();

        if (card.buildingData.Build_Type == 0)
            return;
        
        if ((card.Level) % 10 == 9 || (card.Level) == 0)
        {
            BuffMaster curBuff = buffMasters.Find(x => x.BuffCode == card.currentBuff);
        
            //빌딩 버프
            BuildingManager.instance.buildingsBuffUpdate(card.code, curBuff.BuffUp, curBuff.BuffValueType);
        }
    }

    //전체 빌딩 업데이트
    public void BuildingPriceDisCount()
    {
        for (int i = 0; i < buildingCardList.Count; i++)
        {
            buildingCardList[i].UpdateData();
        }
    }
    
}
