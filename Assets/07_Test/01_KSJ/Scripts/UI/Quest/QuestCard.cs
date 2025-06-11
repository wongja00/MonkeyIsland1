using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestCard : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI questCompanyName;
    
    public string companyName;
    
    [SerializeField]
    TextMeshProUGUI rewardText;
    
    public int reward;
    
    [SerializeField]
    TextMeshProUGUI extraRewardText;
    
    public int extraReward;
    
    [SerializeField]
    TextMeshProUGUI refreshCountText;

    public int refreshCount;
    
    [SerializeField]
    Transform questItemParent;
    
    [SerializeField]
    MarketCard questItemPrefab;
    
    [SerializeField]
    ProductCard prefapbProductCard;
    
    MarketCard[] randomItem = new MarketCard[3];

    public int itemCounts = 0;
    
    public int sellItemCounts = 0;
    
    private void Start()
    {
        Init();
    }
    
    public void Init()
    {
        InitQuestItems();
        
        UpdateUI();
        
        MarketUIManager.addProductCallBack += UpdateItemCount;
        
        questCompanyName.text = companyName;
    }

    public void InitQuestItems()
    {
        //데이터 컨테이너에서 아이템 무작위 3개로 뽑아서 생성
        List<int> radnItemIndex = new List<int>();

        if(randomItem[0] != null)
        {
            for (int i = 0; i < 3; i++)
            {
                Destroy(randomItem[i].gameObject);
            }
        }
        
        //중복안되게
        for (int i = 0; i < 3; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = UnityEngine.Random.Range(0, DataContainer.Instance[DataContainerSetID.Product].Count-1);
            } while (radnItemIndex.Contains(randomIndex));

            radnItemIndex.Add(randomIndex);
        }
        
        for (int i = 0; i < 3; i++)
        {
            ProductCard productCard = prefapbProductCard; 
            productCard.init(DataContainerSetID.Product, DataContainer.Instance[DataContainerSetID.Product][radnItemIndex[i]].ID);
            
            randomItem[i] = Instantiate(questItemPrefab, questItemParent);
            randomItem[i].init(productCard);
            
            randomItem[i].count = UnityEngine.Random.Range(1, 10);
            randomItem[i].UpdateData();
            randomItem[i].Count.text = randomItem[i].count.ToString();
        }
        
        reward = UnityEngine.Random.Range(1, 10) * 1000;
        extraReward = UnityEngine.Random.Range(1, 10) * 1000;

        UpdateUI();
    }
    
    public void UpdateUI()
    {
        rewardText.text = "+" + NumberRolling.ConvertNumberToText(reward);
        extraRewardText.text = "+" + NumberRolling.ConvertNumberToText(extraReward).ToString();
        refreshCountText.text = refreshCount.ToString();

        UpdateItemCount();
    }
    
    public void UpdateItemCount()
    {
        for (int i = 0; i < 3; i++)
        {
            int count = MarketUIManager.instance.GetProductCountByID(randomItem[i].ID);
            if(count >= randomItem[i].count)
            {
                randomItem[i].maskImage.gameObject.SetActive(false);
            }
            else
            {
                randomItem[i].maskImage.gameObject.SetActive(true);
            }
            
            randomItem[i].Count.text = count + "/" + randomItem[i].count.ToString();
        }
    }

    public void Delivery()
    {
        //MarketUIManager.instance.marketCards에 있는 아이템과 비교해서 전부 개수가 충분하면 배송
        //배송시 reward와 extraReward를 획득
        
        bool isAllItemEnough = true;
        
        for (int i = 0; i < 3; i++)
        {
            MarketCard marketCard = MarketUIManager.instance.marketCards.Find(card => card.ID == randomItem[i].ID);
            
            if (marketCard == null || marketCard.count < randomItem[i].count)
            {
                isAllItemEnough = false;
                return;
            }
        }
        
        if (isAllItemEnough)
        {
            for (int i = 0; i < 3; i++)
            {
                MarketCard marketCard = MarketUIManager.instance.marketCards.Find(card => card.ID == randomItem[i].ID);
                marketCard.count -= randomItem[i].count;
                marketCard.Count.text = marketCard.count.ToString();

                itemCounts += randomItem[i].count;
                
                Destroy(randomItem[i].gameObject);
            }
            sellItemCounts += itemCounts;
            itemCounts = 0;
            ShopManager.UsePaymentCount(PaymentType.Gold, reward);
            
            InitQuestItems();
            
            // ChallengesManager.instance.SetChallengeLevel("Sell_Count", sellItemCounts);
            //
            // if (companyName.Contains("카멜"))
            // {
            //     ChallengesManager.instance.SetChallengeLevel("Camel_Sell_Count", sellItemCounts);
            // }
            // if (companyName.Contains("이글"))
            // {
            //     ChallengesManager.instance.SetChallengeLevel("Egle_Sell_Count", sellItemCounts);
            // }
            // if (companyName.Contains("라이언"))
            // {
            //     ChallengesManager.instance.SetChallengeLevel("Raion_Sell_Count", sellItemCounts);
            // }
            
            
        }
    }
    
    public void Refresh()
    {
        QuestUIManager.instance.RefreshQuest(this);
    }
    
}
