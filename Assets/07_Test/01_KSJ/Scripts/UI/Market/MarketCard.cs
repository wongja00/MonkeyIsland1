using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[Serializable]
public class MarketCard : MonoBehaviour
{
    //UI ¿¬°á ºÎ
    public TextMeshProUGUI Name;
    public Image Thumbnail;
    public TextMeshProUGUI Time;
    public TextMeshProUGUI Price;
    public TextMeshProUGUI Count;
    public Image  maskImage;
    
    public int price;
    
    public int count = 1;
    
    public ProductCard ProductData;

    public string ID;
    
    public void init(ProductCard _product)
    {
        ProductData = _product;
        count = 1;
        ID = _product.ProductID;
        
        UpdateData();
    }
    
    public void UpdateData()
    {
        Name.text = ProductData.ProductData.Name;
        Thumbnail.sprite = DataContainer.Instance.productContainer[ID].Thumbnail;
        Count.text = count.ToString();
    }
    
    public void Purchase()
    {
        //MarketUIManager.instance.ProgressSell(this);
    }
    
    public void AddCount()
    {
        count++;
        Count.text = count.ToString();
    }
}
