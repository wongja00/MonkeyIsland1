using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class StorageUIManager : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public Image Thumbnail;
    public Image Guage;
    public TextMeshProUGUI GuageText;
    public TextMeshProUGUI Price;

    public DataContainerSetID SetID;
    int Level;

    StorageChest StorageChest;
    
    void Start()
    {
        UpdateData();
        
        StorageChest = StorageManager.Instance.FindStorageByName(SetID.ToString());
    }

    public void UpdateData()
    {
        Level = DataManager.GetStorageLevel(SetID);
        
        StorageContaner UnitContaner = (DataContainer.Instance[SetID][Level] as StorageContaner);

        if (Level < 0)
        {
            Debug.LogError("창고 데이터가 없음");
        }
        else if (DataContainer.Instance[SetID].Count - 1 > Level)
        {
            //GuageText.text = $"{UnitContaner.MaxStorage}";
            //Title.text = UnitContaner.Name;
            Thumbnail.sprite = UnitContaner.Thumbnail;
            //Price.text = (DataContainer.Instance[SetID][Level + 1] as StorageContaner).Price.ToString();
        }
        else if (DataContainer.Instance[SetID].Count - 1 == Level)
        {
           // GuageText.text = $"{UnitContaner.MaxStorage}";
            //Title.text = UnitContaner.Name;
            Thumbnail.sprite = UnitContaner.Thumbnail;
            Price.text = "MAX";
        }
    }

    public void UpdateGuage()
    {
        StorageContaner UnitContaner = (DataContainer.Instance[SetID][Level] as StorageContaner);
        //UpdateGuage(StorageChest.BananaCount / UnitContaner.MaxStorage);
    }
    public void UpdateGuage(float Value)
    {
        Guage.fillAmount = Value;
    }

    public void Purchase()
    {
        //ShopManager.StoragePurchase(SetID, () => UpdateData());
    }
}
