using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class RebirthSkill : MonoBehaviour
{
    //스킬 버튼
    public Button skillButton;
    
    //스킬 이름
    public TextMeshProUGUI skillName;
    
    //스킬 설명
    public TextMeshProUGUI skillDescription;
    
    //스킬 가격
    public TextMeshProUGUI skillPrice;
    
    //스킬 이미지
    public Image skillImage;
    
    //스킬 레벨
    public TextMeshProUGUI skillLevel;
    
    //스킬 트리
    public SkillTree skillTree;
    
    //스킬 활성화 여부
    public bool isSkillActive;
    
    //스킬ID
    public string skillID;

    private void Awake()
    {
        
    }
 
    private void UpdateSkill()
    {
        
    }
}
