using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductCard : MonoBehaviour
{
    public ProductType ProductType;

    public Productor productor;
    
    //UI ¿¬°á ºÎ
    public TextMeshProUGUI Name;
    public TextMeshProUGUI BananaCount;
    public Image Thumbnail;
    public TextMeshProUGUI Time;
    public TextMeshProUGUI Price;
    
    DataContainerSetID SetID;
    public string ProductID;
    
    [SerializeField]
    private GameObject reserveMask;

    public float time;
    
    public uint bananaValue;
    
    public int price;
    
    public ProductContaner ProductData;

    public ProductCard(ProductContaner productContaner)
    {
        SetID = DataContainerSetID.Product;
        ProductID = productContaner.ID;
        ProductData = productContaner;
        price = (int)ProductData.Price;
    }
    public ProductCard()
    {
        
    }
    
    public void init(DataContainerSetID _SetID, string _ProductID)
    {
        SetID = _SetID;
        ProductID = _ProductID;
        ProductData = (DataContainer.Instance[SetID][ProductID] as ProductContaner);
        UpdateData();
        UpdateDataUI();
    }
    
    private ProductType GetProductType(ProductContaner ProductData)
    {
        string name = ProductData.ID;
        if (name.Contains("Juice")) return ProductType.Drink;
        if (name.Contains("Dessert")) return ProductType.Dessert;
        if (name.Contains("Cook")) return ProductType.Cook;
        return ProductType.Drink;
    }

    public void UpdateDataUI()
    {
        if(Name == null || BananaCount == null || Thumbnail == null || Time == null || Price == null) return;
        
        Name.text = ProductData.Name;
        BananaCount.text =  NumberRolling.ConvertNumberToText(bananaValue).ToString();
        Price.text = "+" + NumberRolling.ConvertNumberToText(price).ToString();
        Time.text = time.ToString("F2");
    }

    public void UpdateData()
    {
        time = ProductData.Time;
        price = (int)ProductData.Price;
        bananaValue = ProductData.BananaCount;
        ProductType = GetProductType(ProductData);
        if(Thumbnail != null)Thumbnail.sprite = ProductData.Thumbnail;
    }
    
    public void Purchase(bool isReserve)
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        if (isReserve)
        {
            if (productor.reservedProduct?.Name != Name)
            {
                UniTask<bool> task = waitBananaCount(cts.Token);
                productor.ReserveProduct(this);
                SetReserveMask(true);
            }
        }
        else
        {
            cts.Cancel();
            productor.ReserveProduct(productor.currentProduct);
            SetReserveMask(false);
        }
    }
    
    public async UniTask<bool> waitBananaCount(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (ShopManager.GetPaymentCount(PaymentType.Banana) >= bananaValue)
            {
                return true;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), true);
        }
        return false;
    }
    
    public void SetReserveMask(bool isReserve)
    {
        reserveMask.SetActive(isReserve);
    }
}
