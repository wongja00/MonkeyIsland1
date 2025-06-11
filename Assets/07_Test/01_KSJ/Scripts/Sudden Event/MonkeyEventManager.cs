using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EventType
{
    Talk,
    Reward
}

public enum EventActionType
{
    Environment,
    MonkeyTalk,
    Post
}

public enum EventRewardType
{
    Banana,
    Diamond,
    MonkeyDollar,
    HarvestBuff,
    GoldBuff,
    AdRemove,
    MiniGameTicket,
    AutoTouch,
}

//원숭이 대화 이벤트(idle)
public class MonkeyEventManager : MonoBehaviour
{
    public static MonkeyEventManager instance;
    
    public List<MonkeyIdleEvent> idleEvents;

    public EventMessagePopUp eventMessagePopUp;
    
    public float eventTime = 6f;

    private double reward;
    
    private Coroutine monkeyEventCoroutine = null;
    
    public List<StringMaster> stringMasters;
    
    public List<WindowMaster> windowMasters;
    
    public List<Dialogue> dialogues;
    
    private List<RewardMaster> rewardMasters = new List<RewardMaster>();

    public Action OntalkEventComplete;
    
    private MonkeyIdleEvent curEvent;
    
    public int EventCount { get; set; }
    
    private void Awake()
    {
        idleEvents = new List<MonkeyIdleEvent>();
        instance = this;
        EventCount = 0;
    }

    private void Start()
    {
        stringMasters = CSVReader.GetStringMaster(EventActionType.MonkeyTalk);
        windowMasters = CSVReader.GetWindowMaster(EventActionType.MonkeyTalk);
        dialogues = CSVReader.GetDialogueData();
        rewardMasters = CSVReader.GetRewardData(EventActionType.MonkeyTalk);

        eventMessagePopUp.rewardPopUp.OnGetReward.AddListener(() => { GetReward(curEvent);});
        
        Init();
    }

    public void Init()
    {
        monkeyEventCoroutine = StartCoroutine(OnMonkeyEventCoroutine());
    }
    
    public IEnumerator OnMonkeyEventCoroutine()
    {
        yield return new WaitForSeconds(eventTime);
        OnMonkeyEvent();
    }
    
    //5분에 한번씩 실행
    public void OnMonkeyEvent()
    {
        int eventCount = 0;
        //현재 이벤트 중인 원숭이가 2개 초과일 경우 이벤트 발생하지 않음
        idleEvents.ForEach(idleEvent =>
        {
            if (idleEvent.isEvent)
            {
                eventCount++;
            }
        });
        if(eventCount > 2 || idleEvents.Count <= eventCount)
        {            
            StopCoroutine(monkeyEventCoroutine);
            monkeyEventCoroutine = StartCoroutine(OnMonkeyEventCoroutine());
            return;
        }
        
        //랜덤으로 하나의 이벤트 발생 - 이벤트 초기화 구문
        int randomIndex = 0;

        if (idleEvents.Count > 0)
        {
            do
            {
                randomIndex = UnityEngine.Random.Range(0, idleEvents.Count);
            } while (idleEvents[randomIndex].isEvent);
        }
 
        if (idleEvents[randomIndex] != null)
        {
            var dialogueGroup = dialogues.Where(d => int.Parse(d.DialogueCode)+15 == idleEvents[randomIndex].eventID).ToList();
            var weightedDialogues = dialogueGroup.Select(d => new WeightedProbability(d.Result, d.Rate)).ToList();

            WeightedProbabilityCalculator calculator = new WeightedProbabilityCalculator();
            var calculatedDialogues = calculator.CalculateProbabilities(weightedDialogues);

            double randomValue = UnityEngine.Random.value;
            double cumulativeProbability = 0.0;
            string selectedDialogue = null;

            int rewardindex = randomIndex;
            
            foreach (var dialogue in calculatedDialogues)
            {
                cumulativeProbability += dialogue.Probability;
                if (randomValue < cumulativeProbability)
                {
                    selectedDialogue = dialogue.Item;

                    if (selectedDialogue.Split("_")[1] == "20")
                    {
                        rewardindex = 15;
                    }
                    break;
                }
            }
            
            idleEvents[randomIndex].curWindowMaster = null;
            
            RewardMaster rewardMaster = rewardMasters[rewardindex];
            
            if (selectedDialogue != null)
            {
                // 선택된 대화 사용
                windowMasters.ForEach(w =>
                {
                    if (w.ChatCode == selectedDialogue)
                    {
                        idleEvents[randomIndex].curWindowMaster = w;
                    }
                });
            }
            
            //TODO: 보상 타입 설정
            int randomReward = UnityEngine.Random.Range(0, EventRewardType.GetNames(typeof(EventRewardType)).Length - 3);
            string[] typeStrings = idleEvents[randomIndex].curWindowMaster.ChatCode.Split("_");
            
            EventType type = (typeStrings[1] == "30") ? EventType.Reward : EventType.Talk;
            EventRewardType rewardType = (EventRewardType)randomReward;
            
            
            
            idleEvents[randomIndex].Init(type, rewardType, rewardMaster);
            idleEvents[randomIndex].OnMonkeyEvent();
            
            StopCoroutine(monkeyEventCoroutine);
            monkeyEventCoroutine = StartCoroutine(OnMonkeyEventCoroutine());
        }
    }
    
    public void ShowPopUp(MonkeyIdleEvent monkeyIdleEvent, EventType eventtype, EventRewardType rewardType)
    {
        curEvent = monkeyIdleEvent;
        
        eventMessagePopUp.button.onClick.AddListener(()=>NextString(monkeyIdleEvent));

        stringMasters.ForEach(s =>
        {
            if (s.StringCode == monkeyIdleEvent.curWindowMaster.ChatText)
            {
                monkeyIdleEvent.curStringMaster = s;
            }
        });
        
        if (monkeyIdleEvent.curWindowMaster.ChatNext == "0")
        {
            eventMessagePopUp.SetButtonText("닫기");
        }
        else
        {
            eventMessagePopUp.SetButtonText("다음");
        }
        
        eventMessagePopUp.ShowPopUp(monkeyIdleEvent);
        eventMessagePopUp.SetDescription(monkeyIdleEvent.curStringMaster.Desc);
        eventMessagePopUp.SetImage(monkeyIdleEvent.eventSprite);   
        
        EventCount++;
        OntalkEventComplete?.Invoke();
    }

    public void NextString(MonkeyIdleEvent eventMonkey)
    {
         if (eventMonkey.curWindowMaster.ChatNext == "0")
         {
             //GetReward(eventMonkey);
             
             //return;
         }
        
        windowMasters.ForEach(w =>
        {
            if (w.ChatCode == eventMonkey.curWindowMaster.ChatNext)
            {
                eventMonkey.curWindowMaster = w;
            }
        });
        
        stringMasters.ForEach(s =>
        {
            if (s.StringCode == eventMonkey.curWindowMaster.ChatText)
            {
                eventMonkey.curStringMaster = s;
            }
        });
        
        eventMessagePopUp.SetDescription(eventMonkey.curStringMaster.Desc);
        
        if (eventMonkey.curWindowMaster.ChatNext == "0")
        {
            // if(eventMonkey._eventType == EventType.Reward)
            {
                eventMessagePopUp.ShowRewardPopUp();
                
                string bananareward = "\n 골드" + NumberRolling.ConvertNumberToText(MonkeyDollorManager.instance.MonkeyDollorperSec * (1 + eventMonkey.rewardValue)/100).ToString();
                string goldreward = "\n 바나나" + NumberRolling.ConvertNumberToText(BananaManager.instance.BananaperSec * (1 + eventMonkey.rewardValue)/100).ToString();
                
                eventMessagePopUp.rewardPopUp.SetRewardPopUpText(bananareward + goldreward);
                
                return;
            }
            
            eventMessagePopUp.SetButtonText("닫기");
        }
        else
        {
            eventMessagePopUp.SetButtonText("다음");
        }
    }
    
    public void GetReward(MonkeyIdleEvent eventMonkey)
    {
       ShopManager.UsePaymentCount(PaymentType.Gold, (MonkeyDollorManager.instance.MonkeyDollorperSec * (1 + eventMonkey.rewardValue)/100));
       ShopManager.UsePaymentCount(PaymentType.Banana, (BananaManager.instance.BananaperSec * (1 + eventMonkey.rewardValue)/100));

        HidePopUp();
        
        StopCoroutine(monkeyEventCoroutine);
        monkeyEventCoroutine = StartCoroutine(OnMonkeyEventCoroutine());
    }

    public void HidePopUp()
    {
        eventMessagePopUp.GetReward();
        eventMessagePopUp.button.onClick.RemoveAllListeners();
    }
}