using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ����ǰ ����Ʈ(�κ��丮)
public struct InventoryProduct
{
    public ProductCard productCard;
    public int count;
}

public class ProductManager : MonoBehaviour
{
    public static ProductManager Instance = null;
    
    private PruductBuilding productBuilding;
    
    //������ ����ǰ ����Ʈ
    private List<InventoryProduct> productList = new List<InventoryProduct>();
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    //������ ����ǰ ����Ʈ�� �߰�
    public void AddProduct(ProductCard productCard)
    {
        var product = productList.Find(p => p.productCard == productCard);
        if (product.productCard != null)
        {
            product.count++;
            productList[productList.IndexOf(product)] = product;
        }
        else
        {
            productList.Add(new InventoryProduct { productCard = productCard, count = 1 });
        }
    }
    
    
}
