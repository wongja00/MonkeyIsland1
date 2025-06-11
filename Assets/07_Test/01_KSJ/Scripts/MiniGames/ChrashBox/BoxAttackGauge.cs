using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAttackGauge : MonoBehaviour
{
    private int maxAttack = 100;

    private double currentAttack;

    [SerializeField]
    private UnityEngine.UI.Image attackGauge;

    public void Init(int maxAttack)
    {
        currentAttack = 0;
        this.maxAttack = maxAttack;   
    }

    public void SetAttackGauge(int attack)
    {
        currentAttack = attack;
        
        attackGauge.fillAmount = (float)currentAttack / maxAttack;
    }
    
    public double GetCurrentAttack()
    {
        return 100 * ( 1 + currentAttack / 100d);
    }
    

}
