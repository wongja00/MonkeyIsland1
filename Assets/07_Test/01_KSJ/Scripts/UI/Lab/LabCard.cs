using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LabCard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelText;
    public int level = 0;

    [SerializeField]
    private Image thumbnail;
    
    public string skillName;
    
    public string skillID;
    
    public int skillDescriptoin;

    public string description;
    
    public int cost;
    
    public void Init()
    {
        
    }
    
    public void UpdateUI()
    {
        levelText.text = level.ToString();
    }

    public void UpdateResearchPanel(bool isOpen)
    {
        if(!LabUIManager.instance.researchPanel.activeSelf)
        {
            LabUIManager.instance.researchPanel.SetActive(true);
        }
    }
    
    
    
}
