using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Challenge
{
    public string ID;
    public string Desc;
    public int level;
    public int goalType;
    public int goalValue;
    public int rewardType;
    public int rewardValue;
    
    public double curGoalValue;
    public int curLevel;

    public int MaxLevel = 20;
    
    public Func<bool>  condition;
    
    public ChallengeCard challengeCard;

    public void init(ChallengeCard _challengeCard, Archivement archivementData, Func<bool> _condition = null)
    {
        //파싱한값
        ID = archivementData.ID;
        Desc = archivementData.Name;
        challengeCard = _challengeCard;
        challengeCard.challenge = this;
        level = archivementData.Level;
        goalType = archivementData.GoalType;
        goalValue = archivementData.GoalQuantity;
        rewardType = archivementData.RewardType;
        rewardValue = archivementData.Reward;
        
        //현재값
        curLevel = 0;
        curGoalValue = 0;
        MaxLevel = 20;
        
        condition = _condition;
        
        SetCard();
    }

    void UpdateData(Archivement archivementData)
    {
        ID = archivementData.ID;
        Desc = archivementData.Name;
        level = archivementData.Level;
        goalType = archivementData.GoalType;
        goalValue = archivementData.GoalQuantity;
        rewardType = archivementData.RewardType;
        rewardValue = archivementData.Reward;
        
        SetCard();
    }
    
    public void SetCard(double Count)
    {
        curGoalValue = Count;
        SetCard();
    }
    
    public void SetCard()
    {
        string Titlename = "";
        string dianame = "";
        bool FinishCheck = false;
        bool ActiveReward = false;

        if (curLevel < MaxLevel)
        {
            Titlename = Desc.Replace("n", NumberRolling.ConvertNumberToText(goalValue).ToString());
            
            dianame = rewardValue.ToString();
            
            if (curGoalValue >= goalValue)
                ActiveReward = true;
        }
        else
        {
            Debug.Log("MAX");
            
            Titlename = Desc.Replace("n", NumberRolling.ConvertNumberToText(goalValue).ToString());
            dianame = "MAX"; 
            FinishCheck = true;
        }

        challengeCard.SetCarde(ActiveReward, FinishCheck, Titlename, dianame);
        challengeCard.Progress.text = NumberRolling.ConvertNumberToText(curGoalValue) + "/" + NumberRolling.ConvertNumberToText(goalValue);
        
    }
    public void GetReward()
    {
        curLevel++;

        string nextIndex = "";

        if (curLevel < 19)
        {
            nextIndex = ID.Split('_')[0] + "_" + (curLevel+1);
            
        }
        
        Archivement ac = ChallengesManager.instance.archivementLists.Find(x => x.ID == nextIndex);
        UpdateData(ac);
        
        ShopManager.UsePaymentCount(PaymentType.Diamond, rewardValue);
        DataManager.SetChallengeCount(ID, curLevel);
        SetCard();
    }
    
    public bool CheckCondition()
    {
        if (condition.Invoke())
        {
            return true;
        }
        return false;
    }
}

public class ChallengesManager : MonoBehaviour
{
    public List<Challenge> challenges;
    public ChallengeCard challengeCardPrefab;
    public Transform challengeCardParent;

    public List<Archivement> archivementLists;
    
    public static ChallengesManager instance;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        archivementLists = CSVReader.GetArchivementData();
        
        for (int i = 0; i < archivementLists.Count; i+=20)
        {
            Challenge tempChallenge = new Challenge();
            tempChallenge.init(Instantiate(challengeCardPrefab, challengeCardParent), archivementLists[i]);
            challenges.Add(tempChallenge);
        }
        
        challengeCardPrefab.gameObject.SetActive(false);
    }

    private void Start()
    {
        Init();
        
        ShopManager.OnGoldUpdate += () => { SetChallengeLevel(1, ShopManager.GetPaymentCount(PaymentType.Gold)); };
        ShopManager.OnGoldUpdate += () => { SetChallengeLevel(2, ShopManager.GetPaymentCount(PaymentType.Banana)); };
        ShopManager.OnBuildingPurchase += (DataContainerSetID Id, string sid, int value) => { SetChallengeLevel(3, DataManager.GetBuildingLevelSum()); };
        ShopManager.OnUnitPurchase += (DataContainerSetID id, string sid, int value) => { SetChallengeLevel(4, DataManager.GetUnitLevelSum()); };
        MonkeyEventManager.instance.OntalkEventComplete += () => { SetChallengeLevel(5, MonkeyEventManager.instance.EventCount); };
        BackGroundEventManager.instance.OnEventComplete += () => { SetChallengeLevel(6, BackGroundEventManager.instance.eventCount); };
    }

    void Init()
    {
        foreach (Challenge challenge in challenges)
        {
            //uint tempPointLevel = DataManager.GetChallengeCount(challenge.ID);
            
            //challenge.PointingLevel = ((int)tempPointLevel);
        }
    }

    public void SetChallengeLevel(int ChallengeID, double count)
    {
        for (int i = 0; i < challenges.Count; i++)
            if (ChallengeID == int.Parse(challenges[i].ID.Split('_')[0]))
            {
                challenges[i].SetCard(count);
                return;
            }
    }
   
}
