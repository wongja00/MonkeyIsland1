using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIValueManager : MonoBehaviour
{
    double gold = 0;
    public TextMeshProUGUI goldText;
    
    double bananas = 0;
    public TextMeshProUGUI bananaText;
    
    double companyValue = 0;
    public TextMeshProUGUI companyValueText;
    
    double skillPoint = 0;
    public TextMeshProUGUI skillPointText;
    
    double dia = 0;
    public TextMeshProUGUI DiaText;
    
    double tierUp = 0;
    public TextMeshProUGUI TierUpText;
    
    [SerializeField]
    ParticleSystem goldEffect;
    
    [SerializeField]
    ParticleSystem bananaEffect;
    
    [SerializeField]
    ParticleSystem skillEffect;
    
    [SerializeField]
    ParticleSystem diaEffect;

    void Start()
    {
        ShopManager.OnPayUpdate += (type) =>
        {
            if (type == PaymentType.Gold) GoldUpdate();
        };

        ShopManager.OnPayUpdate += (type) =>
        {
            if (type == PaymentType.Banana) BananasUpdate();
        };

        ShopManager.OnGoldUpdate += CompanyValueUpdate;
    
    
        ShopManager.OnPayUpdate += type =>  
        {
            if (type == PaymentType.SkillPoint) SkillPointUpdate();
        };
            
            
        ShopManager.OnPayUpdate += type =>  
        {
            if (type == PaymentType.Diamond) DiaUpdate();
        };
        
        ShopManager.OnPayUpdate += type =>  
        {
            //if (type == PaymentType.TierUpPoint) TierUpUpdate();
        };
        
        ShopManager.UsePaymentCount(PaymentType.Banana, 1000d);
        ShopManager.UsePaymentCount(PaymentType.Gold, 1000d);
    }
    
    public void GoldUpdate()
    {
        NumberRolling.SpinningNumber(ref gold, ShopManager.GetPaymentCount(PaymentType.Gold), goldText, value => gold = value);
        goldEffect.Play();
    }

    public void BananasUpdate()
    {
        NumberRolling.SpinningNumber(ref bananas, ShopManager.GetPaymentCount(PaymentType.Banana), bananaText, value => bananas = value);
        bananaEffect.Play();
    }
    
    public void CompanyValueUpdate()
    {
        NumberRolling.SpinningNumber(ref companyValue, ShopManager.GetPaymentCount(PaymentType.CompanyValue), companyValueText, value => companyValue = value);
    }
    
    public void SkillPointUpdate()
    {
        NumberRolling.SpinningNumber(ref skillPoint, ShopManager.GetPaymentCount(PaymentType.SkillPoint), skillPointText, value => skillPoint = value);
        skillEffect.Play();
    }

    public void DiaUpdate()
    {
        NumberRolling.SpinningNumber(ref dia, ShopManager.GetPaymentCount(PaymentType.Diamond), DiaText, value => dia = value);
        diaEffect.Play();
    }
    
    public void TierUpUpdate()
    {
        //NumberRolling.SpinningNumber(ref tierUp, ShopManager.GetPaymentCount(PaymentType.TierUpPoint), TierUpText, value => tierUp = value);
    }


}
