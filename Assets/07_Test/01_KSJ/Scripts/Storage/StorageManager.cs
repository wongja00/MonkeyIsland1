using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StorageType
{
    Storage,
    TempStorage,
    BananaPile
}

public class StorageManager : MonoBehaviour
{
    //인스턴스
    private static StorageManager instance;
    
    //바나나 임시 임시 저장소
    private StorageChest bananaTempStorage;
    
    //임시 저장소
    private StorageChest tempStorage;
    
    //찐 창고
    private StorageChest storage;
    
    public static Action<StorageType> OnStorageBananaCountChanged;
    
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

        //콜백 등록
        ShopManager.OnStoragePurchase += UpgradeStorage;
        
        bananaTempStorage = FindStorageByName("BananaPile");
        bananaTempStorage.MaxBananaCount = 999999999;
        
        storage = FindStorageByName("Storage");
        storage.SetID = DataContainerSetID.Storage;
        
        //tempStorage = FindStorageByName("TempStorage");
        //tempStorage.SetID = DataContainerSetID.TempStorage;

 }

    private void Start()
    {
        UpgradeStorage(DataContainerSetID.Storage, DataManager.GetStorageLevel(DataContainerSetID.Storage));

        UpgradeStorage(DataContainerSetID.TempStorage, DataManager.GetStorageLevel(DataContainerSetID.TempStorage));
        
        //tempStorage.AddBananas(ShopManager.GetPaymentCount(PaymentType.TempBanana));
        
    }

    public static StorageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StorageManager>();
                
                if (instance == null)
                {
                    GameObject container = new GameObject("StorageManager");
                    instance = container.AddComponent<StorageManager>();
                }
            }
            
            return instance;
        }
    }
    
    public StorageChest GetStorageByType(StorageType type)
    {
        switch (type)
        {
            case StorageType.Storage:
                return storage;
            case StorageType.TempStorage:
                return tempStorage;
            case StorageType.BananaPile:
                return bananaTempStorage;
            default:
                return null;
        }
    }
    
    public StorageChest FindStorageByName(string name)
    {
        GameObject[] storageChests = GameObject.FindGameObjectsWithTag("Storage");
        
        foreach (GameObject storage in storageChests)
        {
            
            if (storage.gameObject.name == name)
            {
                return storage.GetComponent<StorageChest>();
            }
        }
        
        return null;
    }
    
    public bool AddBananasToStorage(StorageType type, uint count)
    {
        if(GetStorageByType(type).IsStorageFull(count)) return false;
        
        GetStorageByType(type).AddBananas(count);
        //UpdatePaymentCount(type, count);

        return true;
    }

    public bool RemoveBananasFromStorage(StorageType type, uint count)
    {
        if(GetStorageByType(type).IsStorageEmpty(count)) return false;
        
        GetStorageByType(type).RemoveBanana(count);
        //UpdatePaymentCount(type, -count);
        
        return true;
    }

    private void UpdatePaymentCount(StorageType type, long count)
    {
        switch (type)
        {
            case StorageType.Storage:
                ShopManager.UsePaymentCount(PaymentType.Banana, (int)count);
                break;
            case StorageType.TempStorage:
                ShopManager.UsePaymentCount(PaymentType.TempBanana, (int)count);
                break;
            case StorageType.BananaPile:
                ShopManager.UsePaymentCount(PaymentType.TempTempBanana, (int)count);
                break;
        }
        GetStorageByType(type).UpdateGuage();
    }

    private void UpgradeStorage(DataContainerSetID SetID, int Level)
    {
        switch (SetID)
        {
            case DataContainerSetID.Storage:
                storage.UpgrageStorage(Level);
                //ChallengesManager.instance.SetChallengeLevel(3, Level+1);
                break;
            case DataContainerSetID.TempStorage:
                //tempStorage.UpgrageStorage(Level);
                //ChallengesManager.instance.SetChallengeLevel("Temp_Storage_LV",
                    //Level+1);
                break;
            default:
                break;
        }
    }
    
    public float GetDistanceEachStorage(MonkeyKind kind)
    {
        switch (kind)
        {
            case MonkeyKind.PickUp:
            case MonkeyKind.Carry:
                return Mathf.Abs(bananaTempStorage.transform.position.x - storage.transform.position.x);
            
            default: 
                return 0f;
        }
    }
    
}
