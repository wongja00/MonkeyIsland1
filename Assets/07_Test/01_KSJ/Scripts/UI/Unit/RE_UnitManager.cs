using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RE_UnitManager : MonoBehaviour
{
    public static RE_UnitManager instance;
    
    [Header("UI 연결부")] public RE_UnitCard PrefabUnitCard;
    public Transform CarryingUnitParent;
    public Transform HarvestingUnitParent;
    //public Transform PickUpUnitParent;

    //유닛 카드 리스트
    public List<RE_UnitCard> MonkeyUnitList;
    
    //유닛 카드 리스트
    private List<RE_UnitCard> HarvestingUnit;
    private List<RE_UnitCard> CarryingUnit;

    private Action PurchaseCallBack;

    public List<int> HMonkeyOrder = new List<int>();
    public List<int> CMonkeyOrder = new List<int>();

    private static int levelCount = 0;
    
    [SerializeField]
    private UnitQuickCard unitQuickCard;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        HarvestingUnit = new List<RE_UnitCard>();
        CarryingUnit = new List<RE_UnitCard>();
        
        MonkeyUnitList = new List<RE_UnitCard>();
        
        HMonkeyOrder = CSVReader.GetMonkeyHarvestOrder(1);
        CMonkeyOrder = CSVReader.GetMonkeyHarvestOrder(2);
        InitializeUnitCards();
        MonkeyCardOrder(HMonkeyOrder);
        MonkeyCardOrder(CMonkeyOrder);
        
        //LoadUnit();
        
        FindCheapestUnit();
    }

    private void InitializeUnitCards()
    {
        InitializeUnitCards(ref HarvestingUnit, DataContainerSetID.HarvestingUnit, HarvestingUnitParent);
        InitializeUnitCards(ref CarryingUnit, DataContainerSetID.PickUpUnit, CarryingUnitParent);
    }
    
    void LoadUnit()
    {
        //세이브 된 유닛 카드 정보를 불러온다.
        DataContainerSetID[] setIDs = new DataContainerSetID[]
        {
            DataContainerSetID.HarvestingUnit,
            DataContainerSetID.PickUpUnit,
            DataContainerSetID.CarryingUnit
        };
        
        for(int i = 0; i < setIDs.Length; i++)
        {
            for (int j = 0; j < DataContainer.Instance[setIDs[i]].Count; j++)
            {
                int level = DataManager.GetUnitLevel(setIDs[i], DataContainer.Instance[setIDs[i]][j].ID);
                if (level != -1)
                {
                    
                }
            }
        }
    }

    void InitializeUnitCards(ref List<RE_UnitCard> unitCardList, DataContainerSetID setId, Transform parent)
    {
        unitCardList = new List<RE_UnitCard>();
        for (int i = 0; i < 15; i++)
        {
            UnitContaner tempContaner = null;
            if(setId == DataContainerSetID.PickUpUnit || setId == DataContainerSetID.CarryingUnit)
            {
                tempContaner = DataContainer.Instance[setId][CMonkeyOrder[i].ToString()] as UnitContaner;
            }
            else
            {
                tempContaner = DataContainer.Instance[setId][HMonkeyOrder[i].ToString()] as UnitContaner;
            }
            
            if (tempContaner == null)
            {
                continue;
            }
            
            RE_UnitCard tempUnitCard = Instantiate(PrefabUnitCard, parent);
            
            tempUnitCard.init(this, setId, tempContaner.code.ToString());

            tempUnitCard.gameObject.SetActive(false);
            tempUnitCard.lockImage.gameObject.SetActive(true);
            
            if (tempUnitCard.monkeyMaster.Mon_Code ==  1  || tempUnitCard.monkeyMaster.Mon_Code == 16 || tempUnitCard.level > 0)
            {
                tempUnitCard.gameObject.SetActive(true);
                tempUnitCard.lockImage.gameObject.SetActive(false);
            }

            
            unitCardList.Add(tempUnitCard);
            MonkeyUnitList.Add(tempUnitCard);
        }
    }

    private void MonkeyCardOrder(List<int> MonkeyOrder)
    {
        for (int i = 1; i < MonkeyOrder.Count; i++)
        {
            foreach (RE_UnitCard card in MonkeyUnitList)
            {
                if (card.monkeyMaster.Mon_Code == MonkeyOrder[i - 1])
                {
                    card.nextCardCode = MonkeyOrder[i];
                }
            }
        }
    }
    
    public void OpenUnitCard(int code)
    {
        foreach (RE_UnitCard card in MonkeyUnitList)
        {
            if (card.monkeyMaster.Mon_Code == code)
            {
                card.gameObject.SetActive(true);
            }
        }
    }

    public void UnlockUnitCard(int code)
    {
        foreach (RE_UnitCard card in MonkeyUnitList)
        {
            if (card.monkeyMaster.Mon_Code == code)
            {
                card.lockImage.gameObject.SetActive(false);
            }
        }
    }
    
    public void Purchase(DataContainerSetID SetID, string UnitID,ref int Level, PaymentType _paymentType = PaymentType.Gold, double _price = 0d)
    {
        if (ShopManager.UnitPurchase(SetID, UnitID, Level, PurchaseCallBack, _paymentType, _price))
        {
            levelCount++;
            Level += 1;
            // ChallengesManager.instance.SetChallengeLevel(4, levelCount);
        }
    }
    
    //가장 싼 유닛 찾기
    public void FindCheapestUnit()
    {
        RE_UnitCard cheapestUnit = null;
        double cheapestPrice = double.MaxValue;
        foreach (RE_UnitCard unit in MonkeyUnitList)
        {
            if (unit.price < cheapestPrice && (unit.paymentType == PaymentType.Gold ||  unit.paymentType == PaymentType.Banana))
            {
                cheapestPrice = unit.price;
                cheapestUnit = unit;
            }
        }
        
        if (cheapestUnit == null)
        {
            return;
        }
        
        unitQuickCard.UpdateCheap(
            cheapestUnit.Title.text, 
            cheapestUnit.price, 
            cheapestUnit.LevelHireText.text, 
            cheapestUnit.level,
            NumberRolling.ConvertNumberToText(cheapestUnit.finaloutput) +"/초", 
            cheapestUnit.Thumbnail, 
            cheapestUnit.paymentType,
            cheapestUnit.monkeyMaster.Mon_Type == 1 ? PaymentType.Gold : PaymentType.Banana );
   
        if (cheapestUnit.UnitID == unitQuickCard.code)
        {
            return;
        }
        
        unitQuickCard.buttonHold.OnClick.RemoveAllListeners();
        
        unitQuickCard.buttonHold.OnClick.AddListener(() =>
        {
            cheapestUnit.Purchase();
        });
        
        unitQuickCard.buttonHold.OnHoldStart.AddListener(() =>
        {
            cheapestUnit.Purchase();
        });
        
        unitQuickCard.code = cheapestUnit.UnitID;
        
    }
    
}
