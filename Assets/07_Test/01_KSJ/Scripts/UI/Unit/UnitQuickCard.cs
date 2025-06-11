using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitQuickCard :MonoBehaviour
{
    public Image Thumbnail;
    
    [SerializeField]
    private TextMeshProUGUI Title;
    
    [SerializeField]
    private TextMeshProUGUI priceText;

    public double price { get; private set; } = 0;

    [SerializeField]
    private TextMeshProUGUI Levelup;

    [SerializeField]
    private TextMeshProUGUI Level;

    private int level;

    [SerializeField]
    private TextMeshProUGUI incerment;

    public Button button;
    
    [SerializeField]
    private ButtonColorChange btnColor;
    
    [SerializeField] public ButtonHold buttonHold;
    
    private PaymentType paymentType;

    public string code = "0";
    
    //골드
    [SerializeField]
    private Image priceIcon;
    
    //바나나
    [SerializeField]
    private Image priceIcon2;
    
    //수확 포인트
    [SerializeField]
    private Image priceIcon3;
    
    //운반 포인트
    [SerializeField]
    private Image priceIcon4;

    [SerializeField] private Image outPutImage;
    
    [SerializeField] private Image outPutImage2;
    
    
    private void Start()
    {
        ShopManager.OnGoldUpdate += SetButtonColorByGold;
        
        button.interactable = false;
        buttonHold.OnClick.AddListener(()=>ButtonWink.Wink(button.gameObject));
        buttonHold.OnHoldStart.AddListener(()=>ButtonWink.Wink(button.gameObject));
        buttonHold.OnHoldStart.AddListener(PressButton);
    }

    public void UpdateCheap(string title, double _price, string levelup, int _level,string increase, Image thumbnail, PaymentType paymentType, PaymentType OutPutType)
    {
        Title.text = title;
        price = _price;
        this.priceText.text = NumberRolling.ConvertNumberToText(price);
        Levelup.text = levelup;
        level = _level;
        Level.text = "Lv." + (level+1);
        incerment.text = increase;
        Thumbnail.sprite = thumbnail.sprite;

        SetActiveIcon(priceIcon, paymentType == PaymentType.Gold);
        SetActiveIcon(priceIcon2, paymentType == PaymentType.Banana);
        SetActiveIcon(priceIcon3, paymentType == PaymentType.HarvestTierUpPoint);
        SetActiveIcon(priceIcon4, paymentType == PaymentType.CarryTierUpPoint);
        SetActiveIcon(outPutImage, OutPutType == PaymentType.Gold);
        SetActiveIcon(outPutImage2, OutPutType == PaymentType.Banana);
        
        SetButtonColorByGold();
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
    
    public void PressButton()
    {
        if(buttonHold.interactable)
            btnColor.ChangeThirdColor();
    }
    
    void SetActiveIcon(Image icon, bool isActive)
    {
        icon.gameObject.SetActive(isActive);
    }
}
