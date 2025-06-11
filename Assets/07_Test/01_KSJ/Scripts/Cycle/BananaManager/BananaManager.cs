using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//�ٳ���
public class BananaManager : MonoBehaviour
{
    public static BananaManager instance;

    public double baseBanana = 100;
    
    //��Ȯ ������
    //private double harvestperSec;
    
    //��ų ���ʽ� %
    private double skillBananaorperSec;
     
    //���� ���ʽ� %
    private double paidBananaperSec;
    
    //�� �ٳ��� ���귮
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

    //��Ű�޷� ���귮 ������Ʈ
    public void UpdateBananaPerSecond()
    {

        
        //��ų
        skillBananaorperSec = SkillManager.instance.skillBananaperSec;
        
        //����
        paidBananaperSec = 1;
        
        //�� ��Ű�޷� ���귮
        //��Ȯ������
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
    
    //��Ű�޷� ���� - 1�ʴ� ȣ��
    public void BananaProduce()
    {
        ShopManager.UsePaymentCount(PaymentType.Banana, BananaperSec);
        OnChangeBanana?.Invoke();
        
        //1�ʸ��� ȣ��
        Invoke("BananaProduce", 1f);
    }
    
}
