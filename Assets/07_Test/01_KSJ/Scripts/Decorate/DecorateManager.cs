using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DecorateBuff
{
    Banana,
    MonkeyDollor,
    Diamond,
}

public enum DecorateType
{
    Ground,//1 플랫폼
    Water,//2 물 
    TreeDeco,//3 나무 장식
}

public enum AbilityType
{
    BananaBuff,
    GoldBuff,
    DiamondBuff,
    OfflineBuff,
    MonkeyEventBuff,
    EnvironmentBuff,
    EnvironmentPercentBuff,
    ReportBuff,
    ReportTimeBuff,
    MonkeyUpgradeDiscount,
    BuildingUpgradeDiscount,
    TresureBoxBuff,
    RaceBuff
}

public class DecorateManager : MonoBehaviour
{
    public static DecorateManager instance;

    public List<DecorateObject> DecorateData;
    
    public DecorateObject currentDecorateObject;
    
    public DecoCard card;
    
    public List<DecoCard> cardList;

    public GameObject decoPopUp;
    
    private Camera mainCamera;
    

    private void Awake()
    {
        instance = this;
        mainCamera = Camera.main;
    }

    public bool CheckDecoratePlace(DecorateObject decorateObject)
    {
        return decorateObject.CheckDecorate();
    }
    
    public void Decorate(DecorateObject decorateBuff)
    {
        Instantiate(decorateBuff, decorateBuff.transform.position, Quaternion.identity);
        decorateBuff.Decorate();
        DecorateData.Add(decorateBuff);
    }
    
    public void Remove(DecorateObject decorator)
    {
        decorator.Remove();
    }

    private void FixedUpdate()
    {
        if (TileMap.instance == null || mainCamera == null || currentDecorateObject == null)
        {
            return;
        }
        
        if (TileMap.instance.isTileMode)
        {
            if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width ||
                Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
            {
                return;
            }
            
            //마우스 좌표에 있는 타일 찾기
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit == false || hit.collider == null) return;

            DecorateTile tile = hit.collider.GetComponent<DecorateTile>();
            if (tile == null) return;

            currentDecorateObject.decorateTile = tile;
            currentDecorateObject.transform.position = tile.transform.position;
           
            //마우스 클릭
            if (Input.GetMouseButtonDown(0))
            {
                if (!tile.isTile && tile.tileType == currentDecorateObject.decorateType)
                {
                    Decorate(currentDecorateObject);
                }
            }
        }
    }
    
    public void AbilityActive(AbilityType abilityType, int abilityValue)
    {
        switch (abilityType)
        {
            case AbilityType.BananaBuff:
                break;
            case AbilityType.GoldBuff:
                break;
            case AbilityType.DiamondBuff:
                break;
            case AbilityType.OfflineBuff:
                break;
            case AbilityType.MonkeyEventBuff:
                break;
            case AbilityType.EnvironmentBuff:
                break;
            case AbilityType.EnvironmentPercentBuff:
                break;
            case AbilityType.ReportBuff:
                break;
            case AbilityType.ReportTimeBuff:
                break;
            case AbilityType.MonkeyUpgradeDiscount:
                break;
            case AbilityType.BuildingUpgradeDiscount:
                break;
            case AbilityType.TresureBoxBuff:
                break;
            case AbilityType.RaceBuff:
                break;
        }
    }
    
    public void BananaBuff(int value)
    {
        AbilityActive(AbilityType.BananaBuff, 0);
    }
    
    public void OpenDecoPopUp()
    {
        decoPopUp.SetActive(true);
    }
    
    
}
