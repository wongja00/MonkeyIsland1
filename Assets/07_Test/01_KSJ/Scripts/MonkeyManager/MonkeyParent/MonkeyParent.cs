using System;
using DG.Tweening;
using UnityEngine;

public enum MonkeyState
{
    Idle,
    Moving,
    Harvesting,//수확하기
    Collecting,//바나나 집어야 하는 상태
    Transporting,//바나나를 집고 창고(혹은 임시창고)로 옭기는 상태 
}

public class MonkeyParent : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    
    public BoxCollider2D _collider;
    
    public MonkeyState state = MonkeyState.Idle;

    public int MonCode = 0;
    
    public string ID = "";

    public Animator animator;
    
    public int Level = 0;

    //이동속도
    public float Speed = 10f;
    
    //최대 수확량
    public double pow = 1;
   
    //현재 수확향
    public int basepow = 1;
    
    //바나나 나무
    protected static BananaTree targetTree;
    
    //바나나 더미 저장소(임시임시저장소)
    protected static StorageChest targetBananaPile = null;
    
    //창고
    protected static  StorageChest targetStorage = null;
    
    //임시저장소
    protected static StorageChest targetTempStorage = null;
    
    //지금 바나나를 가지고 있는지
    protected bool isCarryingBanana = false;
    
    //지금 구르는 중인지
    public bool isRolling = false;
    
    //애니 아이디
    protected int isCarryHash;

    //휴식 표시
    public MonkeyIdleEvent eventMark;
    
    //원숭이 종류
    public MonkeyKind kind;
    
    //이동 컴포넌트
    public MonkeyCarryMove monkeyCarryMove = null;
    
    //환생 스킬로 인한 원숭이 속도 상승률(시간 단출률 %)
    public float speedEnforcePercent = 0f;
    
    //환생 스킬로 인한 원숭이 수확량 상승률
    public uint harvestAmountEnforce = 0;
    
    //환생 스킬로 인한 원숭이 운반량 상승률
    public uint maxCarryAmountEnforce = 0;

    public double output = 0;
    
    private void Awake()
    {
        InitializeComponents();
        InitializeStorages();
        
        if(eventMark)eventMark.gameObject.SetActive(false);
    }

    private void InitializeComponents()
    {
        spriteRenderer = spriteRenderer ?? GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        isCarryHash = Animator.StringToHash("IsCarry");
    }

    private void InitializeStorages()
    {
        targetBananaPile = StorageManager.Instance.GetStorageByType(StorageType.BananaPile);
        targetStorage = StorageManager.Instance.GetStorageByType(StorageType.Storage);
        targetTempStorage = StorageManager.Instance.GetStorageByType(StorageType.TempStorage);
    }
    
    //각 창고의 거리를 알려주는 함수
    protected static void FindBananaTree()
    {
        // BananaTree이름의 게임 오브젝트 찾기
        if (targetTree == null)
        {
            targetTree = GameObject.Find("BananaTree").GetComponent<BananaTree>();
        }
    }

    protected void MoveTowardsTargetStorage(StorageChest Storage)
    {
        //나중에 스프라이트 바꿔야함        
        if (MonCode == 17 || (MonCode == 16 && isCarryingBanana))
        {
            monkeyCarryMove.FlipSprite(Storage.transform, spriteRenderer, true);
        }
        else
        {
            monkeyCarryMove.FlipSprite(Storage.transform, spriteRenderer);
        }

        monkeyCarryMove.MoveByTime(Storage.transform, CalculateAdjustedSpeed(Storage), () =>
        {
            if (eventMark.isEvent)
            {
                state = MonkeyState.Idle;
                transform.DOKill();
            }
        });
    }
    
    private float CalculateAdjustedSpeed(StorageChest Storage)
    {
        float distance = Mathf.Abs(transform.position.x - Storage.transform.position.x);
        float baseSpeed = Speed * (distance / StorageManager.Instance.GetDistanceEachStorage(kind));
        return baseSpeed * (1 - speedEnforcePercent / 100);
    }
    
    protected bool IsAtTargetStorage(StorageChest Storage)
    {
        //거리에 따라 창고에 도착했는지 확인
        return Mathf.Abs(transform.position.x - Storage.transform.position.x) < (_collider.bounds.size.x / 2 + Storage._collider.bounds.size.x / 2);
    }  
   
    protected void AdjustPivotToBottomCenter()
    {
        Sprite sprite = spriteRenderer.sprite;
        Texture2D texture = sprite.texture;
        if (!texture.isReadable)
        {
            Debug.LogError("Texture is not readable: " + texture.name);
            return;
        }

        Rect rect = sprite.textureRect;
        Color[] pixels = texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);

        int width = (int)rect.width;
        int height = (int)rect.height;

        int bottomY = -1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (pixels[y * width + x].a > 0)
                {
                    bottomY = y;
                    break;
                }
            }
            if (bottomY != -1) break;
        }

        if (bottomY != -1)
        {
            Vector3 newPivot = new Vector3((spriteRenderer.bounds.max.x  - spriteRenderer.bounds.min.x)/2 , (float)bottomY / height, 0);
            Vector3 offset = spriteRenderer.bounds.min - transform.position;
            transform.position -= offset + newPivot;
        }
    }
    

    
}
