using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SerializedDic = AYellowpaper.SerializedCollections;

public enum CompanyType
{
    MoreGold,
    Honor
}

public class MarketUIManager : MonoBehaviour
{
    public static MarketUIManager instance;
    
    [Header("UI 정보 저장")]
    public MarketCard PrefabMarketCard;
    public Transform MarketInventoryParent;
    public Transform MarketParent;
    public List<MarketCard> marketCards;
    
    [SerializedDic.SerializedDictionary("회사이름","회사")]
    public SerializedDic.SerializedDictionary<string, CompanyCard> companyCardsDictionary;

    [SerializeField]
    private TextMeshProUGUI Max;
    
    [SerializeField]
    private TextMeshProUGUI AllPrice;

    [SerializeField]
    private TextMeshProUGUI autoSellTime;
    
    private int AllPriceGold = 0;
    
    public uint MaxSellProduct = 60;
    
    public uint currentProductCount = 0;
    
    public static Action PurchaseCallBack;
    
    public static Action autoPurchaseCallBack;
    
    public static Action addProductCallBack;
    
    public float autoSellTimeValue = 0;

    //누적 판매횟수
    static public int allSellCounts = 0;
    //누적 골드
    static public int allGoldCounts = 0;

    [SerializeField] 
    private Image sellMask;
    [SerializeField] 
    private TextMeshProUGUI sellMaskTimeText;

    private float sellTime = 10.0f;

    public int finalAward;
    
    private void Awake()
    {
        marketCards = new List<MarketCard>();
        if (instance == null)
        {
            instance = gameObject.GetComponent<MarketUIManager>();

            AutoSellTimer();
            
        }
        else
        {
            Destroy(gameObject);
        }


        
        autoSellTimeValue = 10.0f;
        
        currentProductCount = 0;
    }

    void Start()
    {
        companyCardsDictionary.Values.ToList().ForEach(companyCard =>
        {
            companyCard.Init();
        });
        InitProducts();
        UpdateUI();
    }

    void InitProducts()
    {
        Products[] products = (Products[])Enum.GetValues(typeof(Products));
        for (int i = 0; i < products.Length; i++)
        {
            if (DataManager.GetProductCount(products[i].ToString()) > 0)
            {
                ProductContaner productContaner = DataContainer.Instance[DataContainerSetID.Product][i] as ProductContaner;
                ProductCard tempProductCard = new ProductCard(productContaner);
                
                for (int j = 0; j < DataManager.GetProductCount(products[i].ToString()); j++)
                {
                    AddMarketCard(tempProductCard);
                }
            }
        }
    }
    
    public void AddMarketCard(ProductCard productCard)
    {
        if(IsFull())
        {
            return;
        }
        
        MarketCard tempMarketCard = Instantiate(PrefabMarketCard, MarketInventoryParent);
        
        tempMarketCard.init(productCard);
        
        var matchingCard = marketCards.FirstOrDefault(card => card.ID == tempMarketCard.ID);
        if (matchingCard != null)
        {
            marketCards[marketCards.IndexOf(matchingCard)].AddCount();
            Destroy(tempMarketCard.gameObject);
        }
        else
        {
            marketCards.Add(tempMarketCard);
        }

        currentProductCount++;
        
        int _finalAward = 0;
        for (int i = 0; i < marketCards.Count(); i++)
        {
            MarketCard card = marketCards[i];
            
            _finalAward += card.ProductData.price * card.count;
        }
        
        companyCardsDictionary.Values.ToList().ForEach(companyCard =>
        {
            companyCard.UpdateUI();
            companyCard.FinalAwardTextUpdate(_finalAward);
        });
            finalAward = _finalAward;
        
        if (IsFull())
        {
            autoPurchaseCallBack?.Invoke();
        }
        
        UpdateUI();
        addProductCallBack?.Invoke();
    }  
    
    public int GetFinalAward()
    {
        int _finalAward = 0;
        for (int i = 0; i < marketCards.Count(); i++)
        {
            MarketCard card = marketCards[i];
            
            _finalAward += card.ProductData.price * card.count;
        }

        return _finalAward;
    }
 
    public int GetProductCountByID(string ID)
    {
        var matchingCard = marketCards.FirstOrDefault(card => card.ID == ID);

        return matchingCard != null ? matchingCard.count : 0;
    }

    public void SellProducts(CompanyCard company)
    {
        sellMask.gameObject.SetActive(true);
        
        int _finalAward = 0;
        
        for (int i = 0; i < marketCards.Count(); i++)
        {
            MarketCard card = marketCards[i];
            
            _finalAward += card.ProductData.price * card.count;
        }
        
        //효율까지 고려한 최종 골드
        _finalAward = (int)(_finalAward * (company.percent / 100.0f));
        
        foreach (var marketCard in marketCards)
        {
            Destroy(marketCard.gameObject);
        }
        marketCards.Clear();
        
        currentProductCount = 0;
        UpdateUI();
        
        companyCardsDictionary.Values.ToList().ForEach(companyCard =>
        {
            companyCard.FinalAwardTextUpdate(_finalAward);
        });
        
        SellProgressTask(_finalAward, company).Forget();
        
    }

    private void UpdateUI()
    {
        Max.text = $"{currentProductCount}/{MaxSellProduct}";
        AllPriceGold = marketCards.Sum(card => card.ProductData.price);
    }
    
    private void AutoSellTimer()
    {
        //StartCoroutine(AutoSellTimerCoroutine());
    }

    private async UniTask SellProgressTask(int _finalAward, CompanyCard company)
    {
        while (sellTime > 0)
        {
            sellMaskTimeText.text = "판매까지 남은 시간: " + sellTime.ToString("F0") + "초\n" + "최종 획득 골드: " + _finalAward + "G";
            
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            
            sellTime -= 1f;
        }
        
        int sellCount = Math.Min((int)currentProductCount, (int)MaxSellProduct);
        
        ShopManager.UsePaymentCount(PaymentType.Gold, _finalAward);
        
        //데이터 관리
        Products[] products = (Products[])Enum.GetValues(typeof(Products));
        for (int i = 0; i < products.Length; i++)
        {
            if (DataManager.GetProductCount(products[i].ToString()) > 0)
            {
                DataManager.SetProductCount(products[i].ToString(), 0);
            }
        }
        
        //퍼센트 감소하고 다른 회사 퍼센트 증가
        company.percent = Math.Max(70, company.percent - 10);
        
        //다른 회사 퍼센트 증가
        companyCardsDictionary.Values.ToList().ForEach(companyCard =>
        {
            if (companyCard != company)
            {
                companyCard.percent = Math.Min(150, companyCard.percent + 10);
            }
        });
        
        Companys[] companys = (Companys[])Enum.GetValues(typeof(Companys));
        for (int i = 0; i < companys.Length; i++)
        {
            DataManager.SetCompanyPercent(companys[i].ToString(), companyCardsDictionary[companys[i].ToString()].percent);
        }
            
        allSellCounts += sellCount;
        // ChallengesManager.instance.SetChallengeLevel("Porduct_Sell", allSellCounts);
        // ChallengesManager.instance.SetChallengeLevel("Porduct_Sell_Deep", allSellCounts);
        //
        // ChallengesManager.instance.SetChallengeLevel("Porduct_Sell2Gold", allGoldCounts);
        // ChallengesManager.instance.SetChallengeLevel("Porduct_Sell2Gold_Deep", allGoldCounts);
        //
        // PurchaseCallBack?.Invoke();

        sellMask.gameObject.SetActive(false);
        sellTime = 10.0f;
        UpdateUI();
    }
    
    public bool IsFull()
    {
        if(currentProductCount >= MaxSellProduct)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void SetMaxSellProduct(int value)
    {
        MaxSellProduct = (uint)value;
        Max.text = $"{currentProductCount}/{MaxSellProduct}";
    }
    
}
