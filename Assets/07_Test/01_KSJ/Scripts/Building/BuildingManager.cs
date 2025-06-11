using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;

    public List<BuildingCard> buildingCards;

    public Dictionary<int, int> BuffDiclist = new Dictionary<int, int>();
    
    private double buildingBananabuff;
    private double buildingGoldbuff;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    //건물들의 몽키달러 생산량 합 업데이트
    public void buildingsBuffUpdate(int BuffCode, int buffValue, int BuffType)
    {
        bool cardExists = false;

        if(BuffDiclist.ContainsKey(BuffCode))
        {
            BuffDiclist[BuffCode] = buffValue;
            cardExists = true;
        }

        if (!cardExists)
        {
            BuffDiclist.Add(BuffCode, buffValue);
        }

        buildingBananabuff = 0;
        buildingGoldbuff = 0;
        
        foreach (int Value in BuffDiclist.Values)
        {
            if(BuffType == 2)
            {
                buildingBananabuff += Value;
            }
            else if(BuffType == 1)
            {
                buildingGoldbuff += Value;
            }
        }
        
        if(BuffType == 2)
        {
            BananaManager.instance.UpdateBuildingBuff(buildingBananabuff);
            BananaManager.instance.UpdateBananaPerSecond();
        }
        else if(BuffType == 1)
        {
            MonkeyDollorManager.instance.UpdateBuildingBuff(buildingGoldbuff);
            MonkeyDollorManager.instance.UpdateMonkeyDollorPerSecond();
        }
        
    }
}