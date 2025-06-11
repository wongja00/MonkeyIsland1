using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DropBanana : MonoBehaviour
{
    public Collider2D _collider;
    
    //바나나 더미 갯수
    [SerializeField]
    private uint bananaCount;
    
    //현재 타겟팅 됬는지
    private bool isTargeted = false;
    
    //바나나 색 바꿀 
    private SpriteRenderer spriteRenderer;
    
    [SerializeField]
    private Sprite[] bananaSprites;
    
    public uint BananaCount
    {
        get { return bananaCount; }
        set { bananaCount = value; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        
        spriteRenderer = GetComponent<SpriteRenderer>();

        RandomSprite();
    }
    

    public void RemoveBanana(uint count)
    {
        //바나나 더미 감소
        bananaCount -= count;
        
        //바나나 더미가 0이면 오브젝트 삭제
        if (bananaCount <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    public void RandomSprite()
    {
        spriteRenderer.sprite = bananaSprites[UnityEngine.Random.Range(0, bananaSprites.Length)];
    }
    
    public bool CurrentBananaTargeted()
    {
        return isTargeted;
    }
    
    public void TargetBanana()
    {
        isTargeted = true;
    }
    
}
