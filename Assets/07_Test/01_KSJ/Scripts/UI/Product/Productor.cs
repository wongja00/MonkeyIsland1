using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

//상품을 생성하는 클래스
public class Productor : MonoBehaviour
{
    //가공 시간
    public float productTime = 0;

    //생산품 누적 수량
    public int count;
    
    [SerializeField]
    private TextMeshProUGUI nameText;
    
    [SerializeField]
    public TextMeshProUGUI countText;
    
    [SerializeField]
    public TextMeshProUGUI timeText;
    
    [SerializeField]
    private Image ProductThumbnail;
    
    public int gold = 0;

    private uint bananaValue = 0;
    
    public bool isProduct = false;
    
    //생산중인 생산품
    public ProductCard currentProduct = null;
    
    //예약된 생산품
    public ProductCard reservedProduct = null;

    //프로그래스 바
    [SerializeField]
    public Image progressBar;
    
    public Action productCallBack;
    
    private CancellationTokenSource cts;

    public void ProductInit(ProductCard productCard)
    {
        if(reservedProduct != currentProduct)
        {
            count = 0;
        }

        if (reservedProduct == null)
        {
            isProduct = false;
            return;
        }
        
        currentProduct = productCard;
        currentProduct.SetReserveMask(false);
        
        name = productCard.Name.text;
        
        nameText.text = name;
        
        productTime = productCard.time;
        
        timeText.text = productTime.ToString();
        
        gold = currentProduct.price;

        bananaValue = currentProduct.bananaValue;
        
        countText.text = count.ToString();
        
        ProductThumbnail.sprite = productCard.Thumbnail.sprite;
        
        isProduct = true;
        
        Producting();
    }
    
    //생성중
    private async void Producting()
    {
        //시간이 0이 되면 생성 완료
        //생성 완료시 생성품 인벤토리에 추가
        //생산중인 상품 초기화
        //생성해랏 노예야! (찰싹!)   
        cts?.Cancel();
        cts = new CancellationTokenSource();
        await ProductUIManager.instance.DecreaseTime(this, cts.Token);
    }

    //예약
    public void ReserveProduct(ProductCard productCard)
    {
        reservedProduct = productCard;
        
        if(!isProduct)
        {
            productCard.SetReserveMask(false);
            ProductInit(productCard);
        }
    }

    public void AddProductInventory(ProductCard product)
    {
        //생산품 인벤토리에 추가
        MarketUIManager.instance.AddMarketCard(product);
    }
    


}
