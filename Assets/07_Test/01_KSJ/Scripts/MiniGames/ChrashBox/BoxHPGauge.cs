using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BoxHPGauge : MonoBehaviour
{
    private int maxHP = 0;
    
    [SerializeField]
    private Image hpGauge;
    
    public void Init(int maxHP)
    {
        this.maxHP = maxHP;   
    }
    
    public void SetHPGauge(float hp)
    {
        hpGauge.fillAmount = (float)hp / maxHP;
    }
    
    public void SetColor(Color color)
    {
        hpGauge.color = color;
    }
}
