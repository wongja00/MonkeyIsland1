using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StorageCard : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI MaxBanana;
    public TextMeshProUGUI Gold;
    public Image Thumbnail;

    public DataContainerSetID SetID;

    private int enforceValue = 1;
    
    private List<StorageData> storageDatas = new List<StorageData>();
    
    int indexLevel = 0;
    
    private void Start()
    {
        //storageDatas = CSVReader.ReadStorageCSV(SetID);
        UpdateData();
        
        ShopManager.OnStoragePurchase += (SetID, level) =>
        {
            if (SetID == this.SetID)
            {
                UpdateData();
            }
        };
    }

    public void UpdateData()
    {
        int level = DataManager.GetStorageLevel(SetID);

        if (level <= 79 && level >= 30)
        {
            indexLevel = 1;
        }
        else if (level >= 80)
        {
            indexLevel = 2;
        }
        
        StorageContaner storageContainer = DataContainer.Instance[SetID][0] as StorageContaner;
        
        Storages storageData = storageContainer[indexLevel];
         
        Thumbnail.preserveAspect = true;
        Thumbnail.sprite = storageData.Thumbnail;
        Title.text = storageData.Name + (level >= 0 ? "Lv. " + (level + 1).ToString() : "");
        
        //Thumbnail.color = level < 0 ? Color.black : Color.white;
        MaxBanana.text = storageDatas[level].Max.ToString();
        
        Gold.text = storageDatas[level].Price.ToString() + "G";
        if(level >= 99)
        {
            Gold.text = "MAX";
        }
    }
    
    public void Purchase()
    {
        enforceValue = UnitEnforceValue.enforceValue;
        
        //enforceValue
        int tempLevel = DataManager.GetStorageLevel(SetID) + enforceValue;
        
        if (tempLevel > 99)
        {
            enforceValue = 99 - DataManager.GetStorageLevel(SetID);
        }
        
        //ShopManager.StoragePurchase(SetID, enforceValue);
        
        UpdateData();
    }
}
