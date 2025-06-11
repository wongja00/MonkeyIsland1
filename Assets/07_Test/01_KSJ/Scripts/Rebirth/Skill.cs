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
    
    //스킬 해제
    public void UnlockSkill()
    {
        isUnlocked = true;
    }
    
    //스킬 업그레이드
    public void UpgradeSkill()
    {
        Level++;
    }
    
    //스킬 적용
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
