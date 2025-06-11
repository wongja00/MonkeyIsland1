using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class BananaTree : MonoBehaviour, ClickableObject
{
    private BoxCollider2D _collider;

    [SerializeField]
    private GameObject dropBananaObject;
    
    private StorageChest bananaPile;
    
    private Vector3 OriginPosition;
    
    private DamageLog damageLog;
    
    public uint clickBanana;
    
    public TreeSpine treeSpine;
    
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();

        damageLog = GetComponent<DamageLog>();
        
        bananaPile = StorageManager.Instance.GetStorageByType(StorageType.BananaPile);
        
        OriginPosition = transform.position;
        
        clickBanana = 5;
    }

    public void HarvestBananas(uint banana = 0)
    {
        TreeShake();
        
        //damageLog.ShowDamageLog(banana.ToString());
        
        //바나나 떨어뜨리기
        if(dropBananaObject != null)
        {
            //collider 안에서 여러개 랜덤으로 생성 후 Dotween으로 곡선을 그리며 떨어뜨림
            for (int i = 0; i < 8; i++)
            {
                GameObject dropBanana = Instantiate(dropBananaObject, new Vector3(Random.Range(_collider.bounds.min.x, _collider.bounds.max.x), Random.Range(_collider.bounds.min.y, _collider.bounds.max.y), 0), Quaternion.identity);
                
                //사방으로 터지는 효과
                dropBanana.transform
                        .DOMove(
                            new Vector3(
                                Random.Range(dropBanana.transform.position.x - 0.5f,
                                    dropBanana.transform.position.x + 0.5f),
                                Random.Range(dropBanana.transform.position.y - 0.5f,
                                    dropBanana.transform.position.y + 0.5f), 0), Random.Range(0.1f, 0.3f))
                        .SetEase(Ease.Linear).onComplete =
                    () =>
                    {
                        //시간이 아닌 거리로 떨어뜨리기
                        dropBanana.transform.DOMoveY(-10, 3).SetEase(Ease.Linear).onUpdate = () =>
                        {
                            if (dropBanana != null && dropBanana.transform.position.y <= bananaPile.transform.position.y)
                            {
                                dropBanana.transform.DOKill();
                                Destroy(dropBanana);
                            }
                        };
                    };
            }
        }
        
    }
    
    public void TreeShake()
    {
        //좌우로 흔들게 함
        //transform.DOShakePosition(0.5f, new Vector3(0.02f, 0, 0), 10, 90, false).onComplete = () =>
        //{
        //    transform.position = OriginPosition;
        //};
        
        treeSpine.TreeShake();
    } 
    
    void ClickableObject.OnClick()
    {
        //HarvestBananas(clickBanana);
        
        //StorageManager.Instance.AddBananasToStorage(StorageType.BananaPile, clickBanana);
    }
    
}
