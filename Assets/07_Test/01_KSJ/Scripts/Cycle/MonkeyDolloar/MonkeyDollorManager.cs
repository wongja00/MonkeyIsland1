using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class MonkeyDollorManager : MonoBehaviour
{
    public static MonkeyDollorManager instance;
    
    //운반 원숭이
    //private double transportMonkeyDollorperSec;
    
    //스킬 보너스 %
    private double skillMonkeyDollorperSec;
     
    //유료 보너스 %
    private double paidMonkeyDollorperSec;
    
    //총 몽키달러 생산량
    public double MonkeyDollorperSec;
    
    public Action OnChangeMonkeyDollor;
    
    public List<MonkeyParent> transportMonkeys = new List<MonkeyParent>();
    
    [SerializeField] private TextMeshProUGUI perSecText;

    private double buildingBuff;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        
        //transportMonkeyDollorperSec = 0;
        skillMonkeyDollorperSec = 0;
        paidMonkeyDollorperSec = 1;
        MonkeyDollorperSec = 0;
        buildingBuff = 0;
        
    }

    private void Start()
    {
        UpdateMonkeyDollorPerSecond();
        MonkeyDollorProduce();
    }
    
    public void UpdateBuildingBuff(double buffValue)
    {
        buildingBuff = buffValue;
    }

    //몽키달러 생산량 업데이트
    public void UpdateMonkeyDollorPerSecond()
    {
        //스킬
        skillMonkeyDollorperSec = SkillManager.instance.skillMonkeyDollorperSec;
        
        //유료
        paidMonkeyDollorperSec = 1;

        MonkeyDollorperSec = 0;
        
        //운반원숭이
        for(int i = 0; i < transportMonkeys.Count; i++)
        {
            MonkeyDollorperSec += transportMonkeys[i].output;
        }
        
         MonkeyDollorperSec *= (1 + skillMonkeyDollorperSec/100d);
         MonkeyDollorperSec *= (1 + buildingBuff/100d);
         
        perSecText.text = NumberRolling.ConvertNumberToText(MonkeyDollorperSec) + "/초";
    }
    
    //몽키달러 생산 - 1초당 호출
    public void MonkeyDollorProduce()
    {
        ShopManager.UsePaymentCount(PaymentType.Gold, MonkeyDollorperSec);
        
        OnChangeMonkeyDollor?.Invoke();
        
        //1초마다 호출
        Invoke("MonkeyDollorProduce", 1f);
    }

    private async UniTask MonkeyDollorProducePerSec()
    {
        while (true)
        {
            ShopManager.UsePaymentCount(PaymentType.Gold, MonkeyDollorperSec);
            
            OnChangeMonkeyDollor?.Invoke();
            
            // 1초마다 호출
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            MonkeyDollorProduce();
        }
    }
}
