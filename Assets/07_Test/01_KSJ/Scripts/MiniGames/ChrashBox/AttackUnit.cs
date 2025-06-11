using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUnit : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    
    [SerializeField]
    private BoxAttackGauge boxAttackGauge;
    
    private int currentAttack;

    public void Init()
    {
        currentAttack = 0;
        boxAttackGauge.Init(100);
        StartCoroutine(AttackCoroutine());
    }
    
    private IEnumerator AttackCoroutine()
    {
        while (currentAttack < 100)
        {
            yield return new WaitForSeconds(1f);
            currentAttack += 100/60;
            boxAttackGauge.SetAttackGauge(currentAttack);
        }
    }
}
