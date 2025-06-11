using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

public class MissionCard : MonoBehaviour
{
    //�̼��̸�
    [SerializeField]
    TextMeshProUGUI missionNameText;
    
    public string missionName;
     
    //���α׷�����
    [SerializeField]
    TextMeshProUGUI progressText;// n/n
    
    [SerializeField]
    Image progressBar;
    
    //����
    [SerializeField]
    TextMeshProUGUI rewardText;
    
    //���� ��ų����Ʈ
    public int reward;
    
    public ButtonColorChange btnColor;
    
    public MissionType missionType;
    
    [SerializeField]
    private Mission mission;
    
    public MissionData missionData;
    
    public int progress;
    
    public int goal;
    
    public bool isComplete;

    public int Level = 0;
    
    public void Init(MissionData _mission)
    {
        missionData = _mission;
        Level = _mission.Level;
        mission.InitMission(_mission);
        missionName = mission.missionName;
        
        goal = mission.goal * Level;
        reward = mission.reward;
        
        missionType = _mission.goalType;
        
        missionName = missionName.Replace("N", NumberRolling.ConvertNumberToText(goal).ToString());
        
        //isComplete = mission.isComplete;
        
        // GoalCheck();
        UpdateUI();
    }

    public void GoalCheck(int _progress)
    {
        progress =  _progress;
        
        UpdateUI();
        mission.UpdateMission();
    }
    
    public void UpdateUI()
    {
        missionNameText.text = missionName;
        progressText.text = NumberRolling.ConvertNumberToText(progress) + "/" + NumberRolling.ConvertNumberToText(goal);
        progressBar.fillAmount = (float)progress / (goal);
        rewardText.text = NumberRolling.ConvertNumberToText(reward);
        mission.UpdateMission();
        
        
        if(progress >= goal)
        {
            btnColor.ChangeFirstColor();
        }
        else
        {
            btnColor.ChangeSecondColor();
        }
    }
    
    //���� ����
    public void GetReward()
    {
        if (progress >= goal )
        {
            MissionManager.instance.GetReward();
        }
    }

}
