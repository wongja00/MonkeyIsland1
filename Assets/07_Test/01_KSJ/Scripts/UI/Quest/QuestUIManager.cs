using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    public static QuestUIManager instance;
    
    [SerializeField]
    TextMeshProUGUI questRefreshTimeText;
    public float questRefreshTime;
    
    [SerializeField]
    TextMeshProUGUI questRefreshticketText;
    public int questRefreshTicket;
    public int maxquestRefreshTicket;
    
    public static Action QuestCallBack;
    
    public static int allQuestCount = 0;
    
    public static int camelQuestCount = 0;
    public static int eagleQuestCount = 0;
    public static int lionQuestCount = 0;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
        
        Init();
    }
    
    public void Init()
    {
        maxquestRefreshTicket = 3;
        
        questRefreshTicket = 1;
        
        questRefreshTime = 10;

        StartCoroutine(DecreaseTime());
        
        UpdateUI();
    }
    
    public void RefreshQuest(QuestCard questCard)
    {
        if(questRefreshTicket > 0)
        {
            questRefreshTicket--;

            questCard.InitQuestItems();

            UpdateUI();
            
            if(questRefreshTicket == maxquestRefreshTicket-1)
            {
                StartCoroutine(DecreaseTime());
            }
        }
    }
    
    public void UpdateUI()
    {
        //다음 충전까지...00:00
        questRefreshTimeText.text = "다음 충전까지..." +
                                    Convert.ToInt32(Math.Floor(questRefreshTime / 60)).ToString("D2") + ":" +
                                    Convert.ToInt32(Math.Floor(questRefreshTime % 60)).ToString("D2");
        questRefreshticketText.text = questRefreshTicket.ToString() + " / " + maxquestRefreshTicket.ToString();
    }
    
    private IEnumerator DecreaseTime()
    {
        while(questRefreshTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            
            questRefreshTime--;
            
            UpdateUI();
        }
        
        questRefreshTicket++;
        
        questRefreshTime = 10;
        
        UpdateUI();
        
        if(questRefreshTicket < maxquestRefreshTicket)
        {
            StartCoroutine(DecreaseTime());
        }
    }
}
