using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

public class BuildingCard : MonoBehaviour
{
    //버튼 홀드컴포
    public ButtonHold buttonHold;
        
    //버튼
    [SerializeField]
    protected Button enforceButton;
    
    //버튼 색
    public ButtonColorChange btnColor;

    public DataContainerSetID SetID;
    
    public BuildMaster buildingData;
    
    public BuildingCard nextBuilding;
    
    [SerializeField]
    protected Image thumbnail;

    [SerializeField]
    protected TextMeshProUGUI levelText;
    public int Level = 0;
    
    public int code;
    
    [SerializeField]
    protected TextMeshProUGUI nameText;
    public string _name;
    
    //결제 타입
    private PaymentType paymentType;
    
    //가격
    [SerializeField]
    protected TextMeshProUGUI priceText;
    public double price;
    
    //골드 아이콘
    [SerializeField]
    private Image priceIcon;
    //바나나 재화 아이콘
    [SerializeField]
    private Image priceIcon2;
    
    
    //구매 or 레벨업
    [SerializeField]
    protected TextMeshProUGUI LevelUpText;
    public string LevelUpString;

    //생산 타입(수확, 운반)
    public int BuildType;
    
    //승습재료생산량
    [SerializeField]
    protected TextMeshProUGUI BuildValueText;
    protected int BuildValue;

    //생산시간
    protected int BuildTime;
    private PaymentType pointType;
    
    //버프리스트
    protected List<int> buffList = new List<int>();

    public int currentBuff { get; private set; }

    public void Init(BuildMaster _buildingData)
    {
        buildingData = _buildingData;
        SetID = DataContainerSetID.Building;
        
        code =  buildingData.Build_Code;
        
        Level = DataManager.GetBuildingLevel(buildingData.Build_Code.ToString());
        _name = buildingData.Build_Name;
        nameText.text = _name + " Lv." + Level;
        thumbnail.preserveAspect = true;
        thumbnail.sprite = (DataContainer.Instance[SetID][buildingData.Build_Code.ToString()] as BuildingContaner)[0].Thumbnail;
        
        LevelUpString = "구매";
        
        BuildType = buildingData.Build_Type;
        
        BuildValue = buildingData.Build_Value;

        if (_buildingData.Build_UpType == 1)
        {
            paymentType = PaymentType.Gold;
            priceIcon2.gameObject.SetActive(false);
        }
        else if (_buildingData.Build_UpType == 2)
        {
            paymentType = PaymentType.Banana;
            priceIcon.gameObject.SetActive(false);
        }
        else
        {
            priceIcon.gameObject.SetActive(false);
            priceIcon2.gameObject.SetActive(false);
        }
        
        if (BuildType == 1)
        {
            pointType = PaymentType.HarvestTierUpPoint;
        }
        else if (BuildType == 2)
        {
            pointType = PaymentType.CarryTierUpPoint;
        }
        
        BuildTime = buildingData.Build_Time;
        
        buffList.Add(_buildingData.Build_Buff1);
        buffList.Add(_buildingData.Build_Buff2);
        buffList.Add(_buildingData.Build_Buff3);
        buffList.Add(_buildingData.Build_Buff4);
        buffList.Add(_buildingData.Build_Buff5);
        buffList.Add(_buildingData.Build_Buff6);
        buffList.Add(_buildingData.Build_Buff7);
        buffList.Add(_buildingData.Build_Buff8);
        buffList.Add(_buildingData.Build_Buff9);
        buffList.Add(_buildingData.Build_Buff10);
        
        currentBuff = buffList[0];
        
        buttonHold.OnClick.AddListener(Purchase);
        buttonHold.OnHoldStart.AddListener(Purchase);
        
        buttonHold.OnClick.AddListener(()=>ButtonWink.Wink(enforceButton.gameObject));
        buttonHold.OnHoldStart.AddListener(()=>ButtonWink.Wink(enforceButton.gameObject));
        UpdateData();
        
        ShopManager.OnGoldUpdate += UpdateData;
        
        buttonHold.OnClick.AddListener(pressButton);
        
    }
    
    public void UpdateData()
    {
        int discount = SkillManager.instance.buildingCostDiscount;
        
        if(buildingData.Build_Price.Count > Level + 1)
        {        
            price = buildingData.Build_Price[Level + 1];
        
            price = (double)(price * (1-discount))/100d;
        
            priceText.text = NumberRolling.ConvertNumberToText(price).ToString();
        }
        
        BuildValueText.text = NumberRolling.ConvertNumberToText(BuildValue) + "/" + BuildTime + "H";
        
        nameText.text = _name;

        if (Level > 1)
        {
            LevelUpString = "레벨업";
        }
        
        LevelUpText.text = LevelUpString;
        
        levelText.text = "Lv." + (Level+1);

        
        currentBuff = buffList[(Level+1)/10];
        
        
        
        if (Level >= buildingData.Build_Price.Count - 1)
        {
            priceText.text = "MAX";
            LevelUpText.gameObject.SetActive(false);
            priceIcon.gameObject.SetActive(false);
        }

        SetButtonColorByGold();
    }
    
    public void Purchase()
    {
        if (buildingData.Build_Price.Count -1 <= Level)
        {
            return;
        }
        
        BuikdingEfoceManager.instance.Enforce(this,ref Level, price, paymentType);
        
        if(buildingData.Build_Type == 0 && Level >= 0)
        {
            priceText.text = "MAX";
            levelText.text = "Lv." + (Level+1);
            
            buttonHold.interactable = false;
            buttonHold.OnClick.RemoveAllListeners();
            btnColor.ChangeSecondColor();
        }
    }
    
    private void SetButtonColorByGold()
    {
        //돈이 부족하면 두번째 충분하면 첫번째 색
        if (DataManager.GetPaymentCount(paymentType) < price || Level >= buildingData.Build_Price.Count - 1)
        {
            buttonHold.interactable = false;
            enforceButton.interactable = false;
            btnColor.ChangeSecondColor();
        }
        else
        {
                btnColor.ChangeFirstColor();
                buttonHold.interactable = true;
        }
    }
    
    public void pressButton()
    {
        if(buttonHold.interactable)
            btnColor.ChangeThirdColor();
    }
    
    public void openBuilding()
    {
        gameObject.SetActive(true);
    }
    
}
