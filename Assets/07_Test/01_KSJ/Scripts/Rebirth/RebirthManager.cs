using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillTree
{
    UnitStatUpgrade,
    BuildingStatUpgrade,
    Etc
}

//환생 시스템
//모든 건물과 원숭이의 스탯을 영구적으로 증가시키는 시스템
public class RebirthManager : MonoBehaviour
{
    //싱글톤
    static RebirthManager instance;
    
    //환생 포인트
    public int rebirthPoint;
    
    //환생 레벨
    public int rebirthLevel;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    //환생 시스템을 초기화
    public void InitRebirth()
    {
        
    }
    
    //환생 시스템을 업데이트
    public void UpdateRebirth()
    {
        
    }
    
    //환생 시스템을 저장
    public void SaveRebirth()
    {
        
    }
    
    //환생 시스템을 불러오기
    public void LoadRebirth()
    {
        
    }
    
    
}
