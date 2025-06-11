using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class MonkeyDollorManager : MonoBehaviour
{
    public static MonkeyDollorManager instance;
    
    //��� ������
    //private double transportMonkeyDollorperSec;
    
    //��ų ���ʽ� %
    private double skillMonkeyDollorperSec;
     
    //���� ���ʽ� %
    private double paidMonkeyDollorperSec;
    
    //�� ��Ű�޷� ���귮
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

    //��Ű�޷� ���귮 ������Ʈ
    public void UpdateMonkeyDollorPerSecond()
    {
        //��ų
        skillMonkeyDollorperSec = SkillManager.instance.skillMonkeyDollorperSec;
        
        //����
        paidMonkeyDollorperSec = 1;

        MonkeyDollorperSec = 0;
        
        //��ݿ�����
        for(int i = 0; i < transportMonkeys.Count; i++)
        {
            MonkeyDollorperSec += transportMonkeys[i].output;
        }
        
         MonkeyDollorperSec *= (1 + skillMonkeyDollorperSec/100d);
         MonkeyDollorperSec *= (1 + buildingBuff/100d);
         
        perSecText.text = NumberRolling.ConvertNumberToText(MonkeyDollorperSec) + "/��";
    }
    
    //��Ű�޷� ���� - 1�ʴ� ȣ��
    public void MonkeyDollorProduce()
    {
        ShopManager.UsePaymentCount(PaymentType.Gold, MonkeyDollorperSec);
        
        OnChangeMonkeyDollor?.Invoke();
        
        //1�ʸ��� ȣ��
        Invoke("MonkeyDollorProduce", 1f);
    }

    private async UniTask MonkeyDollorProducePerSec()
    {
        while (true)
        {
            ShopManager.UsePaymentCount(PaymentType.Gold, MonkeyDollorperSec);
            
            OnChangeMonkeyDollor?.Invoke();
            
            // 1�ʸ��� ȣ��
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            MonkeyDollorProduce();
        }
    }
}
