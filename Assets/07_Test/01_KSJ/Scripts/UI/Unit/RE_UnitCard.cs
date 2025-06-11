using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class RE_UnitCard : MonoBehaviour, UnitCardInterface
{
    RE_UnitManager unitUIManager;

    //UI 연결부
    public TextMeshProUGUI Title;

    public double finaloutput { get; private set; } = 0;

    //몽키달러 생산량
    public TextMeshProUGUI MonkeyDollarOutput;
    //바나나 생산량
    public TextMeshProUGUI BananaOutput;
    //레벨
    public TextMeshProUGUI Level;

    public int level = 0;

    //고용량
    public TextMeshProUGUI HireAmount;
    //레벨업 요구 골드
    public TextMeshProUGUI Gold;
    //레벨업, 고용
    public TextMeshProUGUI LevelHireText;
    //썸넬
    public Image Thumbnail;
    //돈부족할떄 잠금이미지
    public Image lockImage;
    
    public bool isLock = true; 

    DataContainerSetID SetID;

    public string UnitID { get; private set; }

    private int enforceValue = 1;

    public double price  { get; private set; }
    
    public RE_UnitCard nextUnit;
    
    //원숭이 정보
    public MonkeyMaster monkeyMaster;
    
    //고유 인덱스
    public uint monkeyIndex;
    //다음 카드 코드
    public int nextCardCode;
    //최대 고용량
    public uint maxHire;
    //현재 고용량
    public int curHire;
    
    //버튼 홀드컴포
    public ButtonHold buttonHold;

    //버튼 색
    public ButtonColorChange btnColor;

    public PaymentType paymentType { get; private set; } = PaymentType.Banana;

    [SerializeField] private GameObject GoldIconImage;
    [SerializeField] private GameObject BananaIconImage;
    [SerializeField] private GameObject TierUpIconImage;

    [SerializeField] public Button button;

    private Dictionary<int, int> TierDic;
    
    string payDonw = "";
    
    public void init(RE_UnitManager _unitUIManager, DataContainerSetID _SetID, string _UnitID)    
    {
        unitUIManager = _unitUIManager;
        SetID = _SetID;
        UnitID = _UnitID;
        
        UnitContaner unitContainer = DataContainer.Instance[SetID][UnitID] as UnitContaner;
        Units unitData = unitContainer[0];
        
        Thumbnail.preserveAspect = true;
        Thumbnail.sprite = unitData.Thumbnail;
        
        monkeyMaster = CSVReader.ReadMonkeyMasterCSV(_UnitID);
        
        Title.text = monkeyMaster.Mon_Name; 
     
        if (monkeyMaster.Mon_Type == 1)
        {
            MonkeyDollarOutput.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            BananaOutput.transform.parent.gameObject.SetActive(false);
        }
        
        level = DataManager.GetUnitLevel(SetID, UnitID);
        
        if(SetID == DataContainerSetID.HarvestingUnit)
        {
            paymentType = PaymentType.Gold;
        }
        else
        {
            paymentType = PaymentType.Banana;
        }
        payDonw = "고용";
        
        UpdateData();
        
        buttonHold.OnClick.AddListener(Purchase);
        buttonHold.OnHoldStart.AddListener(Purchase);
        
        buttonHold.OnClick.AddListener(()=>ButtonWink.Wink(button.gameObject));
        buttonHold.OnHoldStart.AddListener(()=>ButtonWink.Wink(button.gameObject));
        
        ShopManager.OnGoldUpdate +=UpdateData;
        
        buttonHold.OnClick.AddListener(pressButton);
        
        TierDic = new Dictionary<int, int>();
        TierDic.Add(99, monkeyMaster.Mon_Tier100);
        TierDic.Add(149, monkeyMaster.Mon_Tier150);
        TierDic.Add(199, monkeyMaster.Mon_Tier200);
        TierDic.Add(249, monkeyMaster.Mon_Tier250);
        TierDic.Add(299, monkeyMaster.Mon_Tier300);
    }
    
    public void UpdateData()
    {
        Level.text = "Lv. " + (level +1).ToString();

        int discount = 0;
        if (SetID == DataContainerSetID.CarryingUnit)
        {
            discount = SkillManager.instance.carryMonkeyCostDiscount;
        }
        if (SetID == DataContainerSetID.HarvestingUnit)
        {
            discount = SkillManager.instance.harvestMonkeyCostDiscount;
        }
        
        price = (double)(monkeyMaster.Mon_M_Price[level < 299 ? (level+1) :  299] * (1 - discount / 100.0d));
        
        if (level >= 0)
        {
            RE_UnitManager.instance.OpenUnitCard(nextCardCode);
            
            if(SetID == DataContainerSetID.PickUpUnit)
            {
                paymentType = PaymentType.Gold;
            }
            else
            {
                paymentType = PaymentType.Banana;
            }
            
            payDonw = "레벨업";
        }
        
        //승급
        if(level == 99 || level == 149 || level == 199 || level == 249)
        {
            price = TierDic[level];
            
            if (SetID == DataContainerSetID.HarvestingUnit)
                paymentType = PaymentType.HarvestTierUpPoint;
            else if (SetID == DataContainerSetID.CarryingUnit)
                paymentType = PaymentType.CarryTierUpPoint;
            else if (SetID == DataContainerSetID.PickUpUnit)
                paymentType = PaymentType.CarryTierUpPoint;
            
            
            payDonw = "승급";
        }
        
        double output = 0;// = monkeyMaster.output[level < 299 ? (level+1) :  99];
        
        //승급 증가치
        output = level switch
        {
            < 99 => monkeyMaster.Mon_Up100 * (level + 1d),
            < 149 => monkeyMaster.Mon_Up150 * (level + 1d),
            < 199 => monkeyMaster.Mon_Up200 * (level + 1d),
            < 249 => monkeyMaster.Mon_Up250 * (level + 1d),
            < 300 => monkeyMaster.Mon_Up300 * (level + 1d),
            _ => 0
        };
        
        finaloutput = 0;
        
        finaloutput = monkeyMaster.Mon_Base * (1 + output / 100d);

        if (level < 0)
        {
            finaloutput = 0;
        }
        
        BananaIconImage.gameObject.SetActive(paymentType == PaymentType.Banana);
        GoldIconImage.gameObject.SetActive(paymentType == PaymentType.Gold);
        TierUpIconImage.gameObject.SetActive(paymentType == PaymentType.HarvestTierUpPoint || paymentType == PaymentType.CarryTierUpPoint);
        
        BananaOutput.text = NumberRolling.ConvertNumberToText(finaloutput) + "/초";
        MonkeyDollarOutput.text = NumberRolling.ConvertNumberToText(finaloutput).ToString() + "/초";
        Gold.text = NumberRolling.ConvertNumberToText(price).ToString();
        LevelHireText.text = payDonw;
        
        //레벨이 99이상이면 MAX로 표시
        if (level >= 299)
        {
            LevelHireText.text = "레벨";
            Gold.text = "MAX";
            GoldIconImage.gameObject.SetActive(false);
            BananaIconImage.gameObject.SetActive(false);
        }

        if (level == 0)
        {
            RE_UnitManager.instance.UnlockUnitCard(nextCardCode);
        }
        
        SetButtonColorByGold();
    }
    
    public void Purchase()
    {
        // enforceValue = UnitEnforceValue.enforceValue;
        //
        // int tempLevel = DataManager.GetUnitLevel(SetID, UnitID) + enforceValue;
        //
        // if (tempLevel > 299)
        // {
        //     enforceValue = 299 - DataManager.GetUnitLevel(SetID, UnitID);
        // }
        //
        
        if(ShopManager.GetPaymentCount(paymentType) < price && level > 299)
        {
            return;
        }
        
        unitUIManager.Purchase(SetID, UnitID,ref level, paymentType, price);
        
        UpdateData();
        unitUIManager.FindCheapestUnit();
    }

    public void SetButtonColorByGold()
    {
        if (DataManager.GetPaymentCount(paymentType) < price && level < 299)
        {
            buttonHold.interactable = false;
            btnColor.ChangeSecondColor();
        }
        else
        {
            buttonHold.interactable = true;
            btnColor.ChangeFirstColor();
            
        }
    }
    
    public void pressButton()
    {
        if(buttonHold.interactable)
            btnColor.ChangeThirdColor();
    }

    private void OnEnable()
    {
        //UpdateData();
    }
}