using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

public class BuildingCard : MonoBehaviour
{
    //��ư Ȧ������
    public ButtonHold buttonHold;
        
    //��ư
    [SerializeField]
    protected Button enforceButton;
    
    //��ư ��
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
    
    //���� Ÿ��
    private PaymentType paymentType;
    
    //����
    [SerializeField]
    protected TextMeshProUGUI priceText;
    public double price;
    
    //��� ������
    [SerializeField]
    private Image priceIcon;
    //�ٳ��� ��ȭ ������
    [SerializeField]
    private Image priceIcon2;
    
    
    //���� or ������
    [SerializeField]
    protected TextMeshProUGUI LevelUpText;
    public string LevelUpString;

    //���� Ÿ��(��Ȯ, ���)
    public int BuildType;
    
    //�½������귮
    [SerializeField]
    protected TextMeshProUGUI BuildValueText;
    protected int BuildValue;

    //����ð�
    protected int BuildTime;
    private PaymentType pointType;
    
    //��������Ʈ
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
        
        LevelUpString = "����";
        
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
            LevelUpString = "������";
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
        //���� �����ϸ� �ι�° ����ϸ� ù��° ��
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
