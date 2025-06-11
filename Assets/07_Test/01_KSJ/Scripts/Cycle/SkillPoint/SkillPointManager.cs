using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//바나나
public class SkillPoint : MonoBehaviour
{
    public static SkillPoint instance;
    
    //스킬 포인트
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
