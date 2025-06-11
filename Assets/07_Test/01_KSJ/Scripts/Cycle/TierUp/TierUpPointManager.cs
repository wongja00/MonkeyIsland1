using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TierUpPointManager : MonoBehaviour
{
    public static TierUpPointManager instance;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //unitask를 사용하여 티어업 포인트를 얻는다.
    public async UniTask IncreaseTierUpPoint(PaymentType type, int point, int delay)
    {
        while (true)
        {
            await UniTask.Delay(delay);
    
            ShopManager.UsePaymentCount(type, point);
        }
    }
    
}
