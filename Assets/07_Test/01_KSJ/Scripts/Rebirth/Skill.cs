using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string ID;
    public string Name;
    public string Description;
    public int Level;
    public SkillTree skillTree;
    public bool isUnlocked;
    
    public Skill(string id, string name, string description, int level, SkillTree skillTree)
    {
        ID = id;
        Name = name;
        Description = description;
        Level = level;
        this.skillTree = skillTree;
        isUnlocked = false;
    }
    
    //��ų ����
    public void UnlockSkill()
    {
        isUnlocked = true;
    }
    
    //��ų ���׷��̵�
    public void UpgradeSkill()
    {
        Level++;
    }
    
    //��ų ����
    public void ApplySkill()
    {
        switch (skillTree)
        {
            case SkillTree.UnitStatUpgrade:
                break;
            case SkillTree.BuildingStatUpgrade:
                break;
            case SkillTree.Etc:
                break;
        }
    }
}
