using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCard : MonoBehaviour
{
    //스킬 이름
    [SerializeField]
    private TextMeshProUGUI skillNameText;
    
    public string skillName;
    
    //스킬 레벨
    [SerializeField]
    private TextMeshProUGUI skillLevelText;
    
    public int skillLevel;
    
    //스킬 요구 레벨
    [SerializeField]
    private TextMeshProUGUI skillNeedLevelText;
    
    public int skillNeedLevel;
    
    public SkillType skillType;
    
    //스킬 설명
    [SerializeField]
    private TextMeshProUGUI skillDescriptionText;
    
    public string skillDescription;
    
    //스킬 퍼센트
    [SerializeField]
    private TextMeshProUGUI skillPercentText;
    
    public int skillPercent;

    //스킬 포인트
    [SerializeField]
    private TextMeshProUGUI skillPointText;
    
    public int skillPoint;
    
    public ButtonColorChange buttonColorChange;
    
    //스킬 이미지
    [SerializeField]
    private Image skillImage;

    public string ID;
    
    public Action OnSkillUpdate;
    
    [SerializeField]
    private Button skillButton;
    
    public void Init(SkillData skillData)
    {
        skillName = skillData.skillName;
        skillDescription = skillData.skillDesc;
        skillPercent = skillData.skillValue;
        skillPoint = skillData.levelUpCost;
        
        ID = skillData.ID;
        skillNameText.text = skillName;
        skillLevelText.text = skillLevel.ToString();
        skillDescriptionText.text = skillDescription;
        skillPercentText.text = skillPercent.ToString();
        skillPointText.text = skillPoint.ToString();
        skillType = skillData.skillType;
        
        skillNeedLevel = skillData.needLevel;
        

        string tempType = skillData.ID.Split('_')[0];
        switch (tempType)
        {
            case "MD":
                skillType = SkillType.DollarBonus;
                break;
            case "B":
                skillType = SkillType.BananaBonus;
                break;
            case "BE":
                skillType = SkillType.BuildingEnforce;
                break;
            case "DI":
                skillType = SkillType.MonkeyDollarIncentive;
                break;
            case "BI":
                skillType = SkillType.BananaIncentive;
                break;
            case "MM":
                skillType = SkillType.MonkeyMail;
                break;
            case "MR":
                skillType = SkillType.MonkeyReport;
                break;
        }
        
        
        skillImage.preserveAspect = true;
       
        skillImage.sprite = (DataContainer.Instance[DataContainerSetID.Skill][tempType] as SkillContaner)?.Thumbnail;
        
        ShopManager.OnGoldUpdate += UpdateData;
        
        skillButton.onClick.AddListener(SetChangePressedColor);
        
    }
    
    public void UpdateData()
    {
        skillLevelText.text = "Lv." + skillLevel.ToString();
        skillPercentText.text = skillPercent.ToString() + "%";
        skillPointText.text = skillPoint.ToString();
        
        if((int)DataManager.GetPaymentCount(PaymentType.SkillPoint) >= skillPoint)
        {
            skillButton.interactable = true;
            buttonColorChange.ChangeFirstColor();
        }
        else
        {
            skillButton.interactable = false;
            buttonColorChange.ChangeSecondColor();
        }
    }
    
    private void SetChangePressedColor()
    {
        buttonColorChange.ChangeThirdColor();
    }
    
    
    public void LevelUp()
    {
        if((int)DataManager.GetPaymentCount(PaymentType.SkillPoint) >= skillPoint )
        {
            SkillManager.instance.SkillLevelUp(this);
            OnSkillUpdate?.Invoke();
        }
    }
}
