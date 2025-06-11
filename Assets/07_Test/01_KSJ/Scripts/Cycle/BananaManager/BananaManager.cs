using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//바나나
public class BananaManager : MonoBehaviour
{
    public static BananaManager instance;

    public double baseBanana = 100;
    
    //수확 원숭이
    //private double harvestperSec;
    
    //스킬 보너스 %
    private double skillBananaorperSec;
     
    //유료 보너스 %
    private double paidBananaperSec;
    
    //총 바나나 생산량
    public double BananaperSec;
    
    public Action OnChangeBanana;
    
    public List<MonkeyParent> harvestMonkeys = new List<MonkeyParent>();
    
    private double buildingBuff;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        baseBanana = 100;
        //harvestperSec = 0;
        skillBananaorperSec = 1;
        paidBananaperSec = 1;
        
        BananaperSec = 0;

        buildingBuff = 0;
    }

    private void Start()
    {
        UpdateBananaPerSecond();
        BananaProduce();
    }

    public void UpdateBuildingBuff(double buffValue)
    {
        buildingBuff = buffValue;
    }

    //몽키달러 생산량 업데이트
    public void UpdateBananaPerSecond()
    {

        
        //스킬
        skillBananaorperSec = SkillManager.instance.skillBananaperSec;
        
        //유료
        paidBananaperSec = 1;
        
        //총 몽키달러 생산량
        //수확원숭이
        BananaperSec = 0;
        
        for(int i = 0; i < harvestMonkeys.Count; i++)
        {
            // harvestperSec += harvestMonkeys[i].pow / 100;
            // BananaperSec += harvestMonkeys[i].basepow * (1 + harvestMonkeys[i].pow / 100d);
            BananaperSec += harvestMonkeys[i].output;
        }
        
        BananaperSec *=  (1 + skillBananaorperSec/100d) * paidBananaperSec;
        BananaperSec *=  (1 + buildingBuff/100d);
    }
    
    //몽키달러 생산 - 1초당 호출
    public void BananaProduce()
    {
        ShopManager.UsePaymentCount(PaymentType.Banana, BananaperSec);
        OnChangeBanana?.Invoke();
        
        //1초마다 호출
        Invoke("BananaProduce", 1f);
    }
    
}
