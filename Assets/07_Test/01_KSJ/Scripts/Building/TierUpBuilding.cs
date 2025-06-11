using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TierUpBuilding : MonoBehaviour, ClickableObject
{
    public int Code = 0;
    
    //최대량
    private int MaxTierUpPoint;
    //현재량
    private int CurrentTierUpPoint;
    //증가량
    private int IncreasePoint;
    
    private PaymentType type;
    
    private float delay;
    private float curDelay;

    private bool isFull = false;
    
    [SerializeField]
    private TextMeshProUGUI curTierUpPoint;
    
    [SerializeField]
    private Image tierPointProress;
    
    public void Init(int code, PaymentType pointType,int maxTierUpPoint, int _IncreasePoint, float _delay)
    {
        Code = code;
        MaxTierUpPoint = maxTierUpPoint;
        IncreasePoint = _IncreasePoint;
        delay = _delay;

        curDelay = 0;
        isFull = false;
        
        type = pointType;
        
        IncreaseTierUpPoint(type, IncreasePoint, (int)delay);
        
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        boxCollider.size = spriteRenderer.bounds.size;
        boxCollider.offset = spriteRenderer.bounds.center - transform.position;

        if(curTierUpPoint != null)
            curTierUpPoint.text = CurrentTierUpPoint + " / " + MaxTierUpPoint;
                
        if(tierPointProress != null)
            tierPointProress.fillAmount = (float)CurrentTierUpPoint / MaxTierUpPoint;
    }
    
    //unitask를 사용하여 티어업 포인트를 얻는다.
    public async UniTask IncreaseTierUpPoint(PaymentType type, int point, int delay)
    {
        while (true)
        {
            await UniTask.Delay(60000);
            
            if(isFull)
                continue;
            
            curDelay += 1;
            
            if(curDelay >= delay * 60)
            {
                curDelay = 0;
            
                if (CurrentTierUpPoint < MaxTierUpPoint)
                {
                    CurrentTierUpPoint += point;
                    
                    if(curTierUpPoint != null)
                        curTierUpPoint.text = CurrentTierUpPoint + " / " + MaxTierUpPoint;
                    
                    if(tierPointProress != null)
                        tierPointProress.fillAmount = (float)CurrentTierUpPoint / MaxTierUpPoint;
                }
                else
                {
                    isFull = true;
                }
            }
        }
    }
    
    public void OnClick()
    {
        ShopManager.UsePaymentCount(type, CurrentTierUpPoint);
        CurrentTierUpPoint = 0;
        isFull = false;
        
        if(curTierUpPoint != null)
            curTierUpPoint.text = CurrentTierUpPoint + " / " + MaxTierUpPoint;
                
        if(tierPointProress != null)
            tierPointProress.fillAmount = (float)CurrentTierUpPoint / MaxTierUpPoint;
    }
}
