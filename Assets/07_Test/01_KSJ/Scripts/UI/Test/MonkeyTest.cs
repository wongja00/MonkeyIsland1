using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonkeyTest : MonoBehaviour
{
    
    
    [SerializeField]
    private TMP_Dropdown dropdown;
    
    [SerializeField]
    private TMP_InputField inputField;
    
    [SerializeField]
    private Button plusButton;
    
    [SerializeField]
    private Button minusButton;
    
    [SerializeField]
    private Button equlButton;

    private List<PaymentType> paymentTypes = new List<PaymentType>();

    private double curMonkey;
    
    PaymentType currentPaymentType;
    
    void Start()
    {
        paymentTypes.Clear();
        
        paymentTypes.Add(PaymentType.Banana);
        paymentTypes.Add(PaymentType.Gold);
        paymentTypes.Add(PaymentType.Diamond);
        paymentTypes.Add(PaymentType.SkillPoint);
        paymentTypes.Add(PaymentType.HarvestTierUpPoint);
        paymentTypes.Add(PaymentType.CarryTierUpPoint);
        
        paymentTypes.ForEach(x => dropdown.options.Add(new (x.ToString())));
        
        dropdown.onValueChanged.AddListener(SetMoneyType);
        
        plusButton.onClick.AddListener(() => { SetMoney(1); });
        minusButton.onClick.AddListener(() => { SetMoney(-1); });
        equlButton.onClick.AddListener(() => { SetMoney(0); });
    }

    void SetMoneyType(int index)
    {
        switch (index)
        {
            case 0:
                currentPaymentType = PaymentType.Banana;
                break;
            case 1:
                currentPaymentType = PaymentType.Gold;
                break;
            case 2:
                currentPaymentType = PaymentType.Diamond;
                break;
            case 3:
                currentPaymentType = PaymentType.SkillPoint;
                break;
            case 4:
                currentPaymentType = PaymentType.HarvestTierUpPoint;
                break;
            case 5:
                currentPaymentType = PaymentType.CarryTierUpPoint;
                break;
        }
    }
    
    void SetMoney(short value)
    {
        if (double.TryParse(inputField.text,out curMonkey))
        {
            if(curMonkey < 0)
            {
                curMonkey = 0;
                inputField.text = "0";
            }
        }
        else
        {
            curMonkey = 0;
            inputField.text = "0";
        }
        
        switch (value)
        {
            case 0:
                ShopManager.UsePaymentCount(currentPaymentType, curMonkey - ShopManager.GetPaymentCount(currentPaymentType));
                break;
            case 1:
                ShopManager.UsePaymentCount(currentPaymentType, curMonkey);
                break;
            case -1:
                ShopManager.UsePaymentCount(currentPaymentType, -curMonkey);
                break;
        }
    }
}
