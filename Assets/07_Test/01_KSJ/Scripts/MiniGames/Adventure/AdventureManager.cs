using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum BoxType
{
    HPDown,
    ExReward,
    HRHR
}

public class AdventureManager : MonoBehaviour
{
    public static AdventureManager instance;

    private float time = 60f;
    
    //미니게임을 할동안 꺼버릴 월드 오브젝트
    [SerializeField]
    private Transform worldObjects;
    
    [SerializeField]
    private GameObject rewardUI;
    
    [SerializeField]
    private TextMeshProUGUI bananaRewardText;
    
    [SerializeField]
    private TextMeshProUGUI goldRewardText;
    
    [SerializeField]
    private GameObject miniGameUI;
    
    [SerializeField]
    private List<AdventureSlot> adventureSlots;
    
    [SerializeField]
    private Image boxImage;
    
    [SerializeField]
    private Image UnitImage;

    [SerializeField]
    private TextMeshProUGUI HPText;
    
    [SerializeField]
    private BoxHPGauge HPBar;

    [SerializeField] 
    private EventMessagePopUp GetItemPopUp;
    
    private int curPhase = 0;
    
    private Vector2 boxPos;
    
    private List<GameObject> rewardList = new List<GameObject>();
    
    private List<RewardData> rewardDataList = new List<RewardData>();
    
    private int curHP = 5;

    private int movePoint = 0;
    
    private PaymentType[] rewardTypes = new PaymentType[3];

    private void Awake()
    {
        instance = this;
        
        GetItemPopUp.gameObject.SetActive(false);

        GetItemPopUp.button.onClick.AddListener(PhaseStart);
        GetItemPopUp.button.onClick.AddListener(() => GetItemPopUp.gameObject.SetActive(false));
        
        boxPos = boxImage.rectTransform.position;

        
        rewardTypes = new PaymentType[]
        {
            PaymentType.Banana,
            PaymentType.Gold
        };  
    }

    public void InitMiniGame()
    {
        MiniGameManager.instance.MiniGameStart(true);
        
        //adventureSlots.ForEach(x=>x.Init());
        curHP = 5;
        HPBar.Init(curHP);

        curPhase = 0;
        //scrollMaterial.SetFloat("offset", 0.5f);
        
        GetItemPopUp.button.onClick.RemoveAllListeners();
        GetItemPopUp.button.onClick.AddListener(PhaseStart);
        GetItemPopUp.gameObject.SetActive(false);

        StartCoroutine(BoxCome());
    }
    
    public void GetReward()
    {
        rewardDataList.ForEach(x =>
        {
            switch (x.type)
            {
                case PaymentType.Banana:
                    ShopManager.UsePaymentCount(PaymentType.Banana, (int)x.reward);
                    break;
                case PaymentType.Gold:
                    ShopManager.UsePaymentCount(PaymentType.Gold, (int)x.reward);
                    break;
                case PaymentType.Diamond:
                    ShopManager.UsePaymentCount(PaymentType.Diamond, (int)x.reward);
                    break;
            }
        });
        
        rewardDataList.Clear();
        
        MiniGameManager.instance.MiniGameEnd();
        
        rewardUI.SetActive(true);
        bananaRewardText.text = "바나나 : " + rewardDataList.FindAll(x => x.type == PaymentType.Banana).Count;
        goldRewardText.text = "골드 : " + rewardDataList.FindAll(x => x.type == PaymentType.Gold).Count;
        miniGameUI.SetActive(false);
        
    }
    
    public void SetHP(int _hp)
    {
        curHP += _hp;
        HPText.text = curHP + " / 5";
        HPBar.SetHPGauge(curHP);
    }

    private void OpenGetItemPopUp(BoxType type, int reward)
    {
        GetItemPopUp.gameObject.SetActive(true);

        PaymentType curpaymentType = PaymentType.Banana;
        string rewardName = "";
        
        if (type == BoxType.ExReward)
        {
            curpaymentType = rewardTypes[Random.Range(0, rewardTypes.Length-1)];
            
            switch (curpaymentType)
            {
                case PaymentType.Banana:
                    rewardName = "바나나";
                    break;
                case PaymentType.Gold:
                    rewardName = "골드";
                    break;
            }
        }
            
        switch (type)
        {
            case BoxType.HPDown:
                GetItemPopUp.description.text = "HP가 1 감소합니다.\n남은 HP : " + curHP;
                
                break;
            case BoxType.ExReward:
                GetItemPopUp.description.text = NumberRolling.ConvertNumberToText(reward) + "개의 " + rewardName +"를 획득합니다.";
                
                AddReward(curpaymentType, reward);
                break;
            case BoxType.HRHR:
                GetItemPopUp.description.text = NumberRolling.ConvertNumberToText(reward) + "개의 다이아를 획득합니다."+"\n HP가 1 감소합니다.\n남은 HP : " + curHP;

                AddReward(PaymentType.Diamond, reward);
                break;
            
        }
        
        if(curHP <= 0)
        {
            GetItemPopUp.button.onClick.RemoveAllListeners();
            GetItemPopUp.button.onClick.AddListener(EndAdventure);
        }
        
        GetItemPopUp.buttonText.text = "확인";
    }

    void EndAdventure()
    {
        GetItemPopUp.description.text = "HP가 0이 되어 게임이 종료됩니다.";
        GetItemPopUp.buttonText.text = "모험 끝";
        GetItemPopUp.button.onClick.RemoveAllListeners();
        GetItemPopUp.button.onClick.AddListener(GetReward);
        GetItemPopUp.button.onClick.AddListener(() => GetItemPopUp.gameObject.SetActive(false));
    }

    private void AddReward(PaymentType type, double reward)
    {
        rewardDataList.Add(new RewardData()
        {
            type = type,
            reward = reward
        });
    }
    
    private void GetBoxReward()
    {
        int random = Random.Range(0, Enum.GetValues(typeof(BoxType)).Length-1);
        
        BoxType boxType = (BoxType)random;
        
        switch (random)
        {
            case (int)BoxType.HPDown:
                SetHP(-1);
                OpenGetItemPopUp(boxType, -1);
                break;
            case (int)BoxType.ExReward:
                OpenGetItemPopUp(boxType, 1000);
                break;
            case (int)BoxType.HRHR:
                SetHP(-1);
                OpenGetItemPopUp(boxType, 5);
                break;
        }
    }

    public void PhaseStart()
    {
        GetItemPopUp.gameObject.SetActive(false);
        StartCoroutine(BoxCome());
    }

    private IEnumerator BoxCome()
    {
        boxImage.rectTransform.position = new Vector3(UnitImage.rectTransform.position.x + 3, UnitImage.rectTransform.position.y, UnitImage.rectTransform.position.z);
        boxImage.gameObject.SetActive(true);
        
        while(UnitImage.rectTransform.position.x <= boxImage.rectTransform.position.x)
        {
            boxImage.rectTransform.position -= new Vector3(2 * Time.deltaTime, 0, 0);
            yield return null;
        }
        
        boxImage.gameObject.SetActive(false);
        GetBoxReward();
    }

    struct RewardData
    {
        public PaymentType type;
        public double reward;
    }
}
