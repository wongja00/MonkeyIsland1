using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    //미션 ID
    public string ID;
    
    //미션 이름
    public string missionName;
    
    //미션 진행도
    public int progress;
    
    //미션 목표
    public int goal;
    
    //미션 보상
    public int reward;
    
    //미션 완료 여부
    public bool isComplete;
    
    //미션 초기화
    public void InitMission(MissionData _mission)
    {
        ID = _mission.ID;
        
        missionName = _mission.missionName;
        
        goal = _mission.goal;
        
        reward = _mission.reward;
        
        progress = 0;
        
        isComplete = false;
    }
    
    public void UpdateMission()
    {
        if(progress >= goal)
        {
            isComplete = true;
        }
    }
}
