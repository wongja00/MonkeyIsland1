using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackGroundEventManager : MonoBehaviour
{
    public static BackGroundEventManager instance;

    public EventMessagePopUp eventMessagePopUp;
    
    public RewardPopUp eventProbailityMessagePopUp;

    public Transform eventParent;
    
    public List<Transform> eventPositions;
    
    public BackGroundEventObject backGroundEventObject;
    
    public Action EnvironmentEventComplete;
    
    public List<EnvironmentMaster> environmentMasters;
    public List<StringMaster> environmentStrings;
    private List<RewardMaster> rewardMasters;
    
    public int eventCount = 0;
    
    public Coroutine eventCoroutine = null;
    public Coroutine eventOutCoroutine = null;
    
    //이벤트 발생 시간
    public float eventTime = 600f;
    
    public float eventOutTime = 10f;
    
    public Action OnEventComplete;

    private int[] eventOrder;
    
    private Transform curEventObjet;
    
    private void Awake()
    {
        instance = this;
    }
    
    private void Start()
    {
        
        environmentMasters = CSVReader.GetEnvironmentMasters();
        environmentStrings = CSVReader.GetStringMaster(EventActionType.Environment);
        rewardMasters = CSVReader.GetRewardData(EventActionType.Environment);
        eventOrder = new int[] {6, 10, 12, 14};
        
        eventMessagePopUp.aDButton.onClick.AddListener(OnEnvironmentEventReward);

        eventMessagePopUp.button.onClick.AddListener(() =>
        {
            eventCoroutine = StartCoroutine(OnEvnironmentEventCoroutine());
        });
        
        eventPositions = new List<Transform>();
        
        //eventParent 자식 오브젝트의 위치를 저장
        for (int i = 0; i < eventParent.childCount; i++)
        {
            eventPositions.Add(eventParent.GetChild(i));
        }
        
        eventCoroutine = StartCoroutine(OnEvnironmentEventCoroutine());

    }

    //이벤트 발생
    public void OnEnvironmentEvent()
    {
        if (backGroundEventObject.isEvent)
            return;
        
        int posIndex = Random.Range(0, eventOrder.Length);
        
        int success = Random.Range(1, 100);

        backGroundEventObject.OnEvent();
        backGroundEventObject.Init(environmentMasters[eventOrder[posIndex]]);
        
        // 팝업 메시지
        var environmentMaster = environmentMasters[eventOrder[posIndex]];
        var environmentString = environmentStrings.Find(x => x.StringCode == environmentMaster.ChatText);
        if (environmentString != null)
        {
            string successStirng = $"\n<color=#00FF00>{backGroundEventObject.WindowMaster.ChatSuccessProbability}%</color>";

            eventMessagePopUp.SetDescription(environmentString.Desc + successStirng);
        }

        // 확률에 따른 다음 메시지
        bool isSuccess = environmentMaster.ChatSuccessProbability >= success ;
        string nextCode = isSuccess 
            ? environmentMaster.ChatNextSeccess 
            : environmentMaster.ChatNextFailed;

        var nextEnvironmentMaster = environmentMasters.Find(x => x.ChatCode == nextCode);
        
        if (nextEnvironmentMaster != null)
        {
            var nextEnvironmentString = environmentStrings.Find(y => y.StringCode == nextEnvironmentMaster.ChatText);
            
            if (nextEnvironmentString != null)
            {
                string rewardTypeText = "";
                string rewardValueText = "";
                PaymentType pay;

                if (isSuccess)
                {
                    if (rewardMasters.Find(x => x.Code == eventOrder[posIndex]).Type ==1)
                    {
                        pay = PaymentType.Banana;
                        rewardTypeText = "바나나";
                        rewardValueText = NumberRolling.ConvertNumberToText(rewardMasters.Find(x => x.Code == eventOrder[posIndex]).Value *
                                                                            BananaManager.instance.BananaperSec).ToString();
                        
                        eventProbailityMessagePopUp.rewardPopUpButton.onClick.AddListener(() =>
                        {
                            ShopManager.UsePaymentCount(pay, rewardMasters.Find(x => x.Code == eventOrder[posIndex]).Value *
                                                             BananaManager.instance.BananaperSec);
                        });
                    }
                    else
                    {
                        pay = PaymentType.Gold;
                        rewardTypeText = "골드";
                        rewardValueText = NumberRolling.ConvertNumberToText(rewardMasters.Find(x => x.Code == eventOrder[posIndex]).Value *
                                                                            MonkeyDollorManager.instance.MonkeyDollorperSec).ToString();
                        
                        eventProbailityMessagePopUp.rewardPopUpButton.onClick.AddListener(() =>
                        {
                            ShopManager.UsePaymentCount(pay, rewardMasters.Find(x => x.Code == eventOrder[posIndex]).Value *
                                                             MonkeyDollorManager.instance.MonkeyDollorperSec);
                        });
                    }
                }

                eventProbailityMessagePopUp.rewardPopUpButton.onClick.AddListener(() =>
                {
                    eventCoroutine = StartCoroutine(OnEvnironmentEventCoroutine());
                });
                eventProbailityMessagePopUp.rewardPopUpButton.onClick.AddListener(eventProbailityMessagePopUp
                    .rewardPopUpButton.onClick.RemoveAllListeners);

                eventProbailityMessagePopUp.rewardPopUpButton.onClick.AddListener(eventProbailityMessagePopUp
                    .GetReward);

                eventProbailityMessagePopUp.successMessage = nextEnvironmentString.Desc + "\n" + rewardValueText + rewardTypeText;
            }
        }
        
        eventPositions[eventOrder[posIndex]].gameObject.SetActive(true);
        backGroundEventObject.transform.position = eventPositions[eventOrder[posIndex]].position;
        backGroundEventObject.boxCollider2D.size = eventPositions[eventOrder[posIndex]].GetChild(0).localScale;
        curEventObjet = eventPositions[eventOrder[posIndex]];
        
        if(eventOutCoroutine != null)
         StopCoroutine(eventOutCoroutine);
        
        eventOutCoroutine = StartCoroutine(EventTimeOut());
    }

    public IEnumerator EventTimeOut()
    {
        yield return new WaitForSeconds(eventOutTime);
        
        backGroundEventObject.EventOut();
        curEventObjet.gameObject.SetActive(false);
        
        StopCoroutine(eventCoroutine);
        eventCoroutine = StartCoroutine(OnEvnironmentEventCoroutine());
    }

    public IEnumerator OnEvnironmentEventCoroutine()
    {
        yield return new WaitForSeconds(eventTime);
        OnEnvironmentEvent();
    }
    
    //이벤트 보상창
    public void OnEnvironmentEventReward()
    {
        eventProbailityMessagePopUp.Init(eventMessagePopUp);
        
        
        StartCoroutine(eventProbailityMessagePopUp.TextByTime());
    }

    //이벤트 클릭 이벤트 팝업창 뜸
    public void OnEventInteract(SuddenEventObject suddenEventObject)
    {
        if(eventOutCoroutine != null)
            StopCoroutine(eventOutCoroutine);
        
        eventMessagePopUp.ShowPopUp(suddenEventObject);
        curEventObjet.gameObject.SetActive(false);
        
        eventCount++;
        
        OnEventComplete?.Invoke();
        EnvironmentEventComplete?.Invoke();
    }
}
