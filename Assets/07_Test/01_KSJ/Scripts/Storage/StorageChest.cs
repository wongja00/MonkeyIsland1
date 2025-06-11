using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class StorageChest : MonoBehaviour
{
    public BoxCollider2D _collider;

    public uint BananaCount = 0;
    
    public uint MaxBananaCount = 100;
    
    public StorageProgressBar bananaGauge;
    
    public DataContainerSetID SetID;
    
    public SpriteRenderer spriteRenderer;

    private int Level = 0;
    
    private List<StorageData> storageData;

    private int levelIndex = 0;
    
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        bananaGauge = GetComponentInChildren<StorageProgressBar>();
    }

    private void Start()
    {
        if (SetID == DataContainerSetID.Storage || SetID == DataContainerSetID.TempStorage)
        {
            //InitStorage();
        }

        ShopManager.OnGoldUpdate += UpdateGuage;
    }

    private void InitStorage()
    {
        Level = DataManager.GetStorageLevel(SetID);
        
        StorageContaner unitContainer = (DataContainer.Instance[SetID][0] as StorageContaner);

        if (Level <= 79 && Level >= 30)
        {
            levelIndex = 1;
        }
        else if (Level >= 80)
        {
            levelIndex = 2;
        }
        
        SetMaxBananaCount((uint)storageData[Level].Max);

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = unitContainer[levelIndex].Thumbnail;
        }
        
        if (bananaGauge != null)
        {
            bananaGauge.GaugeInit(BananaCount, MaxBananaCount);
        }
        else
        {
            Debug.LogError("게이지바 못 찾음");
        }
        
    }

    public void UpgrageStorage(int Level)
    {
        
        StorageContaner unitContainer = (DataContainer.Instance[SetID][0] as StorageContaner);
        
        //storageData = CSVReader.ReadStorageCSV(SetID);
        
        if (Level <= 79 && Level >= 30)
        {
            levelIndex = 1;
        }
        else if (Level >= 80)
        {
            levelIndex = 2;
        }
        
        //SetMaxBananaCount((uint)storageData[Level].Max);

        gameObject.name = unitContainer[levelIndex].Name;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = unitContainer[levelIndex].Thumbnail;
        }

        UpdateGuage();
    }
    
    public void SetMaxBananaCount(uint count)
    {
        MaxBananaCount = count;
        
        if(bananaGauge != null)
        {
            bananaGauge.SetMaxGauge(MaxBananaCount);
        }
    }
    
    public bool IsStorageEmpty(uint transportAmount)
    {
        if (BananaCount == 0 || (int)BananaCount - transportAmount < 0)
        {
            return true;
        }
        
        return false;
    }
    
    public bool IsStorageFull(uint transportAmount)
    {
        if (BananaCount >= MaxBananaCount || MaxBananaCount - BananaCount < transportAmount)
        {
            return true;
        }
        
        return false;
    }
    
    public void AddBananas(uint count)
    {
        BananaCount += count;
    }
    
    public void RemoveBanana(uint count)
    {
        BananaCount -= count;
    }
    
    public void UpdateGuage()
    {
        if (bananaGauge != null)
        {
            bananaGauge.SetGauge(BananaCount);
        }
    }
}
