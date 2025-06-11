using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class MonkeyPostEventManager : MonoBehaviour
{
    public static MonkeyPostEventManager instance;
    
    public  EventMessagePopUp eventMessagePopUp;
    public  RewardPopUp RewardMessagePopUp;
    
    //»ÐÇÏ°í ³ª¿Ã ¿ø¼þÀÌ ¿ìÆí
    public MonkeyPost monkeyPost;
    
    //ÀÌº¥Æ® Á¾·ùº°·Î ÇÏ³ª¾¿
    public List<List<WindowMaster>> windowMastersLists;
    
    public List<StringMaster> stringMasters;
    
    //ÀÌº¥Æ® ÁöÁ¡
    public Vector3 eventPoint;
    
    public EventRewardType rewardType;
    
    //ÀÌº¥Æ® ½Ã°£
    public float eventTime = 60;
    public float eventCoolTime = 300;
    
    private WindowMaster curWindowMaster;
    
    private int randomindex = 0;
    
    private int stringIndex = 0;
    
    public Action PostEventComplete;
    
    public int eventCount = 0;
    
    public int reward = 0;

    public double diareward = 0;

    private double bananareward;
    private double goldreward;
    
    private void Awake()
    {
        instance = this;
    }

    
    private void Start()
    {
        List<WindowMaster> tempList = CSVReader.GetWindowMaster(EventActionType.Post);
        
        List<WindowMaster> temp = new List<WindowMaster>();
        windowMastersLists = new List<List<WindowMaster>>();
        
        for(int i =0; tempList.Count > i; i++)
        {
            string[] tempCodes = tempList[i].ChatCode.Split('_');

            if (i != 0)
            {
                string[] preTempCodes = tempList[i - 1].ChatCode.Split('_');
                if (tempCodes[1] != preTempCodes[1])
                {
                    windowMastersLists.Add(temp);
                    temp = new List<WindowMaster>();
                }
                temp.Add(tempList[i]);
            }
            else
            {
                temp.Add(tempList[i]);
            }
            
            if(i == tempList.Count - 1)
            {
                windowMastersLists.Add(temp);
            }
        }
        
        stringMasters = CSVReader.GetStringMaster(EventActionType.Post);
        
        eventMessagePopUp.button.onClick.AddListener(NextString);
        eventMessagePopUp.aDButton.onClick.AddListener(ADManager.instance.LoadRewardedAd);
        eventMessagePopUp.aDButton.onClick.AddListener(AdReward);
        eventMessagePopUp.aDButton.gameObject.SetActive(false);
        
        RewardMessagePopUp.rewardPopUpButton.onClick.AddListener(GetReward);
        
        Invoke("OnEvent", eventTime);
        eventPoint = monkeyPost.transform.position;
    }
    
    public void OnEvent()
    {
        if(monkeyPost.isEvent)
        {
            return;
        }
        
        randomindex = UnityEngine.Random.Range(1, windowMastersLists.Count);

        monkeyPost.OnEvent();
    }

    public void Popup(SuddenEventObject obj)
    {
        stringIndex=0;
        monkeyPost.EventOut();
        monkeyPost.transform.position = eventPoint;
        
        eventMessagePopUp.ShowPopUp(obj);
        
        curWindowMaster = windowMastersLists[randomindex][stringIndex];
     
        eventMessagePopUp.aDButton.gameObject.SetActive(false);
        eventMessagePopUp.SetButtonText("´ÙÀ½");
        
        foreach(StringMaster sM in stringMasters)
        {
            if(curWindowMaster.ChatText == sM.StringCode)
            {
                eventMessagePopUp.SetDescription(sM.Desc);
            }
        }
        
        eventCount++;
        PostEventComplete?.Invoke();
    }

    public void NextString()
    {
        
        if (curWindowMaster.ChatNext == "0")
        {
            eventMessagePopUp.button.onClick.RemoveAllListeners();
            eventMessagePopUp.button.onClick.AddListener(ShowRewardPopUp);
            eventMessagePopUp.aDButton.gameObject.SetActive(true);
            eventMessagePopUp.SetButtonText("´Ý±â");
            return;
        }
        stringIndex++;
        
        stringMasters.ForEach
        (
            x =>{
                if (x.StringCode == windowMastersLists[randomindex][stringIndex].ChatText)
                {
                    curWindowMaster = windowMastersLists[randomindex][stringIndex];
                    eventMessagePopUp.SetDescription(x.Desc);
                }
            }
        );
    }
    
    public void ShowRewardPopUp()
    {
        bananareward = BananaManager.instance.BananaperSec * 60 * 15;
        goldreward = MonkeyDollorManager.instance.MonkeyDollorperSec * 60 * 15;
        
        RewardMessagePopUp.Init(eventMessagePopUp);
        RewardMessagePopUp.SetRewardPopUpText(NumberRolling.ConvertNumberToText(bananareward) + "¹Ù³ª³ª È¹µæ\n" + NumberRolling.ConvertNumberToText(goldreward) + "°ñµå È¹µæ");
        eventMessagePopUp.gameObject.SetActive(false);
    }

    public void AdReward()
    {
        
        bananareward = BananaManager.instance.BananaperSec * 60 * 15 * 3 * (1 + (SkillManager.instance.monkeyMailBonus/100d));
        goldreward = MonkeyDollorManager.instance.MonkeyDollorperSec * 60 * 15 * 3 * (1 + (SkillManager.instance.monkeyMailBonus/100d));
        diareward = 10;
        
        RewardMessagePopUp.Init(eventMessagePopUp);
        RewardMessagePopUp.SetRewardPopUpText(NumberRolling.ConvertNumberToText(bananareward) + "¹Ù³ª³ª È¹µæ\n" + NumberRolling.ConvertNumberToText(goldreward) + "°ñµå È¹µæ" + RewardMessagePopUp.desc.text + $"\n ´ÙÀÌ¾Æ {diareward}°³ È¹µæ");
        eventMessagePopUp.gameObject.SetActive(false);
    }

    public void GetReward()
    {
        reward = curWindowMaster.Reward;

        RewardMessagePopUp.SetRewardPopUpText("");
        
        if (SkillManager.instance.monkeyMailBonus > 0)
        {
            //reward = (int)(reward * (1 + SkillManager.instance.monkeyMailBonus / 100f));
        }
        
        ShopManager.UsePaymentCount(PaymentType.Gold, goldreward);
        ShopManager.UsePaymentCount(PaymentType.Banana, bananareward);
        ShopManager.UsePaymentCount(PaymentType.Diamond, diareward);
        
        
        
        goldreward = 0;
        bananareward = 0;
        diareward = 0;
        
        eventMessagePopUp.button.onClick.RemoveAllListeners();
        eventMessagePopUp.button.onClick.AddListener(NextString);
        eventMessagePopUp.GetReward();
        Invoke("OnEvent", eventTime);
    }
    
    public void EventOut()
    {
        monkeyPost.EventOut();
        Invoke("OnEvent", eventTime);
    }
}
