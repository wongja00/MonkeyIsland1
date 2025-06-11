using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using SerializedDic = AYellowpaper.SerializedCollections;
using Cysharp.Threading.Tasks;

public enum ProductType
{
    Drink,
    Dessert,
    Cook
} 

public class ProductUIManager : MonoBehaviour
{
    public static ProductUIManager instance;
    
    [Header("UI 정보 저장")]
    public ProductCard PrefabProductCard;
    public Transform JuiceProductParent;
    public Transform DessertProductParent;
    public Transform CookProductParent;
    
    [SerializedDic.SerializedDictionary("가게 종류","가게")]
    public SerializedDic.SerializedDictionary<ProductType, Productor> ProductCardDictionary;
    
    public List<ProductCard> ProductCardDatas;
    
    public ProductProgressCard PrefabProductProgressCard;
    public Transform ProductProgressParent;
    
    private DataContainerSetID SetID;
    private string UnitID;
    
    public static Action PurchaseCallBack;
    
    [SerializeField]
    private MarketUIManager marketUIManager;

    public List<ProductCard> staticProductCardDatas;
    
    public static int allProductCounts = 0;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ProductCardDatas = new List<ProductCard>();
        staticProductCardDatas = new List<ProductCard>();
        
        for (int i = 0; i < DataContainer.Instance[DataContainerSetID.Product].Count; i++)
        {
            ProductCard tempProductCard = Instantiate(PrefabProductCard, JuiceProductParent);
     
            tempProductCard.init(DataContainerSetID.Product, DataContainer.Instance[DataContainerSetID.Product][i].ID);

            SetParentProductCard(tempProductCard);
            
            ProductCardDatas.Add(tempProductCard);
            staticProductCardDatas.Add(tempProductCard);
        }
    }
    
    public void UpdateProductCardUI()
    {
        for (int i = 0; i < ProductCardDatas.Count; i++)
        {
            ProductCardDatas[i].UpdateDataUI();
        }
    }
    
    private void SetParentProductCard(ProductCard productCard)
    {
        switch (productCard.ProductType)
        {
            case ProductType.Drink:
                productCard.transform.SetParent(JuiceProductParent);
                productCard.productor = ProductCardDictionary[ProductType.Drink];
                productCard.GetComponent<Toggle>().group = JuiceProductParent.GetComponent<ToggleGroup>();
                break;
            case ProductType.Dessert:
                productCard.transform.SetParent(DessertProductParent);
                productCard.productor = ProductCardDictionary[ProductType.Dessert];
                productCard.GetComponent<Toggle>().group = DessertProductParent.GetComponent<ToggleGroup>();
                break;
            case ProductType.Cook:
                productCard.transform.SetParent(CookProductParent);
                productCard.productor = ProductCardDictionary[ProductType.Cook];
                productCard.GetComponent<Toggle>().group = CookProductParent.GetComponent<ToggleGroup>();
                break;
            default:
                break;
        }
    }

    public async UniTask DecreaseTime(Productor productor, CancellationToken _cancellationToken = default)
    {
        try
        {
            await UniTask.WaitUntil(() =>
            {
            productor.isProduct = false;
            _cancellationToken.ThrowIfCancellationRequested();

            if (ShopManager.UsePaymentCount(PaymentType.Banana, (int)-productor.currentProduct.bananaValue))
            {
                return true;
            }
            return false;
            }, cancellationToken: _cancellationToken);

            
            
            StorageManager.Instance.RemoveBananasFromStorage(StorageType.Storage, productor.currentProduct.bananaValue);

            productor.isProduct = true;

            while (productor.productTime > 0)
            {
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

            productor.productTime -= 0.1f;

            productor.timeText.text = productor.productTime.ToString();

            productor.progressBar.fillAmount = productor.productTime / productor.currentProduct.time;
            }

            await UniTask.WaitUntil(() => !MarketUIManager.instance.IsFull());

            // 시간이 다 되었을 때의 처리
            productor.AddProductInventory(productor.currentProduct);
            int tempCount = (int)DataManager.GetProductCount(productor.currentProduct.ProductID) + 1;
            DataManager.SetProductCount(productor.currentProduct.ProductID, tempCount);

            productor.isProduct = false;        
            productor.count++;
            allProductCounts++;
            productor.countText.text = productor.count.ToString();

            productor.ProductInit(productor.reservedProduct);

            // ChallengesManager.instance.SetChallengeLevel("Porduct_Create", allProductCounts);
            // ChallengesManager.instance.SetChallengeLevel("Porduct_Create_Deep", allProductCounts);
        }
        catch (OperationCanceledException)
        {
            productor.ProductInit(productor.reservedProduct);
        }
    }
    
    public void EnforceProductor(ProductType productType, ProductorData data)
    {
        List<ProductCard> tempCards = ProductCardDatas.FindAll(card => card.ProductType == productType);

        if(tempCards.Count == 0)
        {
            return;
        }
        
        for (int i = 0; i < tempCards.Count; i++)
        {
            tempCards[i].time = tempCards[i].ProductData.Time / data.monkeyDollarperSecond;
            
            //언더 플로어 되면 항상 1로 만들기
            
            if (tempCards[i].ProductData.BananaCount < data.BananaDecrease)
            {
                tempCards[i].ProductData.BananaCount = 1;
            }
            else
            {
                tempCards[i].bananaValue = unchecked(tempCards[i].ProductData.BananaCount - data.BananaDecrease);
            }

            tempCards[i].price = (int)(tempCards[i].ProductData.Price + data.SellValue);
        }
        
        UpdateProductCardUI();
    }
    
}
