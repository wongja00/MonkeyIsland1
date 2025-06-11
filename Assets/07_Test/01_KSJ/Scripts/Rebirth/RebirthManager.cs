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

//ȯ�� �ý���
//��� �ǹ��� �������� ������ ���������� ������Ű�� �ý���
public class RebirthManager : MonoBehaviour
{
    //�̱���
    static RebirthManager instance;
    
    //ȯ�� ����Ʈ
    public int rebirthPoint;
    
    //ȯ�� ����
    public int rebirthLevel;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    //ȯ�� �ý����� �ʱ�ȭ
    public void InitRebirth()
    {
        
    }
    
    //ȯ�� �ý����� ������Ʈ
    public void UpdateRebirth()
    {
        
    }
    
    //ȯ�� �ý����� ����
    public void SaveRebirth()
    {
        
    }
    
    //ȯ�� �ý����� �ҷ�����
    public void LoadRebirth()
    {
        
    }
    
    
}
