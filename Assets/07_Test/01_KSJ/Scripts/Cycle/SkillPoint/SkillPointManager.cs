using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//�ٳ���
public class SkillPoint : MonoBehaviour
{
    public static SkillPoint instance;
    
    //��ų ����Ʈ
    private int skillPoint;
    
    //public Action OnChangeSkillPoint;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        
        skillPoint = 0;

    }

    public void AddSkillPoint(int point)
    {
        skillPoint += point;
        
        ShopManager.UsePaymentCount(PaymentType.SkillPoint, point);
        
        //OnChangeSkillPoint?.Invoke();
    }

}
