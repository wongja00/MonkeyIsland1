using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using DG.Tweening;

public class CrashBox : MonoBehaviour, ClickableObject
{
    [SerializeField]
    private SpriteRenderer boxSpriteRenderer;
    
    private double curHP = 10000;

    private double damage;
    
    [SerializeField]
    private BoxHPGauge boxHPGauge;

    [SerializeField]
    private BoxHPGauge boxAttackCollTimeGauge;

    private bool isOpen = false;
    
    private bool canAttack = true;
    
    [SerializeField]
    private BoxAttackGauge boxAttackGauge;
    
    private float boxOpenTime = 5f;
    
    private float boxOpenInterval = 5f;
    
    [SerializeField]
    private List<Sprite> boxCloseSprites;
    
    [SerializeField]
    private List<Sprite> boxOpenSprites;

    private int boxLevel = 0;
    
    public void Init()
    {
        damage = BananaManager.instance.BananaperSec / 50 * 0.8d;
        curHP = 3000;
        boxHPGauge.Init((int)1000);
        boxAttackCollTimeGauge.Init(5);
        StartCoroutine(DamageCoroutine());
        StartCoroutine(OpenBoxIntervalCoroutine());
        boxLevel = 0;
    }
    
    public void OnClick()
    {
        if (!isOpen)
        {
            return;
        }
        
        if (!canAttack)
        {
            return;
        }
        
        boxSpriteRenderer.DOColor(Color.red, 0.1f).SetLoops(6, LoopType.Yoyo);

        transform.DOShakePosition(0.4f, new Vector3(0.5f, 0, 0), 10, 90, false, true);
        
        Getdamage(boxAttackGauge.GetCurrentAttack());
        StartCoroutine(AttackCoroutine());
    }

    public void Getdamage(double _damage)
    {
        curHP -= _damage;
        Debug.Log("박스 체력 : " + curHP);
        boxHPGauge.SetHPGauge((int)curHP/3);
        
        if (curHP < 1000 * (2 - boxLevel))
        {
            //체력을 다깎았을 떄 보상
            
            if(boxLevel >= 2)
            {
                CrashBoxManager.instance.GetReward(GetCurHPbyPercent());
                return;
            }
            
            boxLevel++;
            
            boxSpriteRenderer.sprite = boxCloseSprites[boxLevel];
            
        }
    }
    
    private IEnumerator DamageCoroutine()
    {
        while (curHP > 0)
        {
            yield return new WaitForSeconds(1f);
            Getdamage(damage);
        }
    }
    
    private IEnumerator OpenBoxCoroutine()
    {
        float curtime = UnityEngine.Random.Range(1f, 7);
        isOpen = true;
        
        boxSpriteRenderer.sprite = boxOpenSprites[boxLevel];
        
        yield return new WaitForSeconds(curtime);
        
        isOpen = false;
        
        
        boxSpriteRenderer.sprite = boxCloseSprites[boxLevel];
        StartCoroutine(OpenBoxIntervalCoroutine());
    }
    
    private IEnumerator OpenBoxIntervalCoroutine()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 5));
        StartCoroutine(OpenBoxCoroutine());
    }
    
    //공격 쿨타임
    private IEnumerator AttackCoroutine()
    {
        float curtime = 0f;
        canAttack = false;
        boxAttackCollTimeGauge.SetHPGauge(0);
        boxAttackCollTimeGauge.SetColor(Color.red);
        
        while (curtime <= 5)
        {
            yield return new WaitForSeconds(0.2f);
            curtime += 0.2f;
            boxAttackCollTimeGauge.SetHPGauge(curtime);
        }
        boxAttackCollTimeGauge.SetColor(Color.green);
        canAttack = true;
    }
    
    //체력에 따른 보상을 위한 함수
    public double GetCurHPbyPercent()
    {
        return (1000-curHP) / 1000;
    }
}
