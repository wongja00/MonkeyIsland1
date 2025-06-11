using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    //�̼� ID
    public string ID;
    
    //�̼� �̸�
    public string missionName;
    
    //�̼� ���൵
    public int progress;
    
    //�̼� ��ǥ
    public int goal;
    
    //�̼� ����
    public int reward;
    
    //�̼� �Ϸ� ����
    public bool isComplete;
    
    //�̼� �ʱ�ȭ
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
