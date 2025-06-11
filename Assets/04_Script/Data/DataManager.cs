using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defective.JSON;
using System.IO;
using System.Linq;

public enum PaymentType { 
    Gold, TempTempBanana, TempBanana, Banana, CompanyValue, Diamond, SkillPoint, HarvestTierUpPoint, CarryTierUpPoint
}

public enum Products
{
    BananaMilk_Juice,
    BananaPud_Dessert,
    BananaJuice_Juice,
    BananaChicken_Cook,
    BananaPie_Dessert,
    BananaChip_Cook,
    BananaMuffin_Dessert,
    BananaDummyJuice_Juice,
    BananaDummyCook_Cook
}

public enum Companys
{
    Camel,
    Lion,
    Eagle
}

public static class DataManager
{
    private static Dictionary<PaymentType, double> totalPayments = new Dictionary<PaymentType, double>();
    
    static private JSONObject JsonData;
    static public void Dataload()
    {
        JsonData = new JSONObject();
        if (File.Exists(Application.persistentDataPath + @"\SaveData"))
            JsonData = new JSONObject(File.ReadAllText(Application.persistentDataPath + @"\SaveData"));
        JsonData = IntegrityCheck(JsonData);
    }
    static private JSONObject IntegrityCheck(JSONObject data)
    {
        JSONObject Data = data;

        for (int i = 0; i < Enum.GetValues(typeof(PaymentType)).Length; i++)
        {
            string paymentType = ((PaymentType)i).ToString(); 
            
            if (!Data.HasField(paymentType))
                Data.AddField(paymentType, 0d);
        }
        
        ItemSetContaner TempItemSetContaner;
        DataContainerSetID[] UnitContainerSets = { 
            DataContainerSetID.CarryingUnit,
            DataContainerSetID.HarvestingUnit,
            DataContainerSetID.PickUpUnit
        };
        for (int num = 0; num < UnitContainerSets.Length; num++)
        {
            TempItemSetContaner = DataContainer.Instance[UnitContainerSets[num]];
            string SetName = UnitContainerSets[num].ToString();
            if (!Data.HasField(SetName))
                Data.AddField(SetName, new JSONObject());
            for (int i = 0; i < TempItemSetContaner.Count; i++)
            {
                if (!Data[SetName].HasField(TempItemSetContaner[i].ID))
                {
                    Data[SetName].AddField(TempItemSetContaner[i].ID, -1);
                }
            }
        }
        
        //°Ç¹°
        if (!Data.HasField(DataContainerSetID.Building.ToString()))
            Data.AddField(DataContainerSetID.Building.ToString(), new JSONObject());
        
        ItemSetContaner TempBuildingSetContaner = DataContainer.Instance[DataContainerSetID.Building];
        for (int i = 0; i < TempBuildingSetContaner.Count; i++)
        {
            string SetName = TempBuildingSetContaner[i].ID;

            if (!Data[DataContainerSetID.Building.ToString()].HasField(SetName))
            {
                if (SetName == "11" || SetName == "12")
                {
                    //Data[DataContainerSetID.Building.ToString()].AddField(SetName, 0);
                    //continue;
                }
                
                Data[DataContainerSetID.Building.ToString()].AddField(SetName, -1);

            }
        }
        
        if (!Data.HasField(DataContainerSetID.Storage.ToString()))
            Data.AddField(DataContainerSetID.Storage.ToString(), 0);
        if (!Data.HasField(DataContainerSetID.TempStorage.ToString()))
            Data.AddField(DataContainerSetID.TempStorage.ToString(), 0);
       
        if (!Data.HasField(DataContainerSetID.JuiceProductor.ToString()))
            Data.AddField(DataContainerSetID.JuiceProductor.ToString(), 0);
        if (!Data.HasField(DataContainerSetID.DessertProductor.ToString()))
            Data.AddField(DataContainerSetID.DessertProductor.ToString(), 0);
        if (!Data.HasField(DataContainerSetID.CookProductor.ToString()))
            Data.AddField(DataContainerSetID.CookProductor.ToString(), 0);
        
        if (!Data.HasField(DataContainerSetID.Market.ToString()))
            Data.AddField(DataContainerSetID.Market.ToString(), 0);

        //ÆÇ¸ÅÃ³ ÀÎº¥Åä¸® (ID, Count)
        Products[] products = (Products[])Enum.GetValues(typeof(Products));
        for (int i = 0; i < products.Length; i++)
        {
            if (!Data.HasField(products[i].ToString()))
                Data.AddField(products[i].ToString(), 0);
        }
        
        Companys[] companys = (Companys[])Enum.GetValues(typeof(Companys));
        for (int i = 0; i < companys.Length; i++)
        {
            if (!Data.HasField(companys[i].ToString()))
                Data.AddField(companys[i].ToString(), 100);
        }
        //  
        // foreach (Challenge challenge in ChallengesManager.instance.challenges)
        // {
        //     if (!Data.HasField(challenge.ID))
        //         Data.AddField(challenge.ID, 0);
        // }
        
        return Data;
    }
    static public void DataSave()
    {
        string stringData = JsonData.ToString();
        File.WriteAllText(Application.persistentDataPath + @"\SaveData", stringData);
    }
    
    static public void DataReset()
    {
        JsonData = new JSONObject();
        IntegrityCheck(JsonData);
        DataSave();
    }

    //µ¥ÀÌÅÍ ºÒ·¯¿À±â
    public static int GetUnitLevel(DataContainerSetID SetID, string UnitID)
    {
        if (JsonData == null)
        {
            Debug.LogError("JsonData is not initialized.");
            return -1;
        }

        if (!JsonData.HasField(SetID.ToString()))
        {
            Debug.LogError($"JsonData does not contain the field: {SetID}");
            return -1;
        }

        if (!JsonData[SetID.ToString()].HasField(UnitID))
        {
            Debug.LogError($"JsonData[{SetID}] does not contain the field: {UnitID}");
            return -1;
        }

        return JsonData[SetID.ToString()][UnitID].intValue;
    } 
    
    //¸ðµç À¯´Ö ·¹º§ ÃÑÇÕ
    public static int GetUnitLevelSum()
    {
        int Level = 0;
        
        DataContainerSetID[] UnitContainerSets = { 
            DataContainerSetID.CarryingUnit,
            DataContainerSetID.HarvestingUnit,
            DataContainerSetID.PickUpUnit
        };
        
        for (int j = 0; j < UnitContainerSets.Length; j++)
        {
             ItemSetContaner TempItemSetContaner = DataContainer.Instance[UnitContainerSets[j]];
            for (int i = 0; i < TempItemSetContaner.Count; i++)
            {
                int temp = JsonData[UnitContainerSets[j].ToString()][TempItemSetContaner[i].ID].intValue;
                
                if(temp == -1)
                    continue;
                    
                temp +=1;
                
                Level += temp;
            }
        }
        return Level;
    }
    
    //¼öÈ® À¯´Ö ·¹º§ ÃÑÇÕ
    public static int GetHarvestLevel()
    {
        int Level = 0;
        ItemSetContaner TempItemSetContaner = DataContainer.Instance[DataContainerSetID.HarvestingUnit];
        for (int i = 0; i < TempItemSetContaner.Count; i++)
        {
            int temp = JsonData[DataContainerSetID.HarvestingUnit.ToString()][TempItemSetContaner[i].ID].intValue;
            if(temp == -1)
                continue;
                
            Level += temp+1;
        }
        return Level;
    }
    
    //¿î¹Ý À¯´Ö ·¹º§ ÃÑÇÕ
    public static int GetCarryLevel()
    {
        int Level = 0;
        ItemSetContaner TempItemSetContaner = DataContainer.Instance[DataContainerSetID.PickUpUnit];

            for (int i = 0; i < TempItemSetContaner.Count; i++)
            {
                int temp = JsonData[DataContainerSetID.PickUpUnit.ToString()][TempItemSetContaner[i].ID].intValue;
                if(temp == -1)
                    continue;
                    
                Level += temp+1;
            }
        return Level;
    }
    
    //¼öÈ® À¯´Ö °¹¼ö
    public static int GetHarvestCount()
    {
        int Count = 0;
        ItemSetContaner TempItemSetContaner = DataContainer.Instance[DataContainerSetID.HarvestingUnit];
        for (int i = 0; i < TempItemSetContaner.Count; i++)
        {
            int temp = JsonData[DataContainerSetID.HarvestingUnit.ToString()][TempItemSetContaner[i].ID].intValue;
            if(temp == -1)
                continue;
                
            Count++;
        }
        return Count;
    }
    
    //¿î¹Ý À¯´Ö °¹¼ö
    public static int GetCarryCount()
    {
        int Count = 0;
        ItemSetContaner TempItemSetContaner = DataContainer.Instance[DataContainerSetID.CarryingUnit];
        for (int i = 0; i < TempItemSetContaner.Count; i++)
        {
            int temp = JsonData[DataContainerSetID.CarryingUnit.ToString()][TempItemSetContaner[i].ID].intValue;
            if(temp == -1)
                continue;
                
            Count++;
        }
        return Count;
    }
    
    public static int GetStorageLevel(DataContainerSetID SetID)
    {
        if (SetID == DataContainerSetID.TempStorage || SetID == DataContainerSetID.Storage)
            return JsonData[SetID.ToString()].intValue;
        return -1;
    }
    
    public static int GetBuildingLevel(string BuildingID)
    {
        if (JsonData == null)
        {
            Debug.LogError("JsonData is not initialized.");
            return -1;
        }

        if (!JsonData.HasField(DataContainerSetID.Building.ToString()))
        {
            Debug.LogError($"JsonData does not contain the field: {DataContainerSetID.Building.ToString()}");
            return -1;
        }
        
        if (!JsonData[DataContainerSetID.Building.ToString()].HasField(BuildingID))
        {
            Debug.LogError($"JsonData does not contain the field: {BuildingID}");
            return -1;
        }
        
        return JsonData[DataContainerSetID.Building.ToString()][BuildingID].intValue;
    }
    
    //ºôµù ·¹º§ ÃÑÇÕ
    public static int GetBuildingLevelSum()
    {
        int Level = 0;
        ItemSetContaner TempItemSetContaner = DataContainer.Instance[DataContainerSetID.Building];
        for (int i = 0; i < TempItemSetContaner.Count; i++)
        {
            int temp = JsonData[DataContainerSetID.Building.ToString()][TempItemSetContaner[i].ID].intValue;
            if(temp == -1)
                continue;
                
            temp +=1;
            
            Level += temp;
        }
        return Level;
    }    
    //ºôµù °¹¼ö
    public static int GetBuildingCount()
    {
        int Count = 0;
        ItemSetContaner TempItemSetContaner = DataContainer.Instance[DataContainerSetID.Building];
        for (int i = 0; i < TempItemSetContaner.Count; i++)
        {
            int temp = JsonData[DataContainerSetID.Building.ToString()][TempItemSetContaner[i].ID].intValue;
            if(temp == -1)
                continue;
                
            Count++;
        }
        return Count;
    }

    
    public static double GetPaymentCount(PaymentType paymentType)
    {
        return (double)JsonData[paymentType.ToString()].doubleValue;
    }

    public static uint GetProductCount(string productID)
    {
        return (uint)JsonData[productID].doubleValue;
    }
    
    public static uint GetChallengeCount(string ChallengeID)
    {
        return (uint)JsonData[ChallengeID].doubleValue;
    }
    
    public static int GetCompanyPercent(string companyName)
    {
        return JsonData[companyName].intValue;
    }

    public static void SetChallengeCount(string ID, int level)
    {
        JsonData.SetField(ID, level);
    }
    
    public static void SetProductCount(string productID, int Count)
    {
        JsonData.SetField(productID, Count);
    }
    
    public static void SetCompanyPercent(string companyName, int percent)
    {
        JsonData.SetField(companyName, percent);
    }
    
    //¾÷±×·¹ÀÌµå ¹× ÀçÈ­ »ç¿ë
    public static bool UnitUpgrade(DataContainerSetID SetID, string UnitID, double price)
    {
        if (SetID == DataContainerSetID.CarryingUnit || SetID == DataContainerSetID.HarvestingUnit || SetID == DataContainerSetID.PickUpUnit)
        {
            int NowLevel = JsonData[SetID.ToString()][UnitID].intValue;
            
            JsonData[SetID.ToString()].SetField(UnitID,NowLevel + 1);
            return true;
        }
        return false;  
    }
    
    public static bool BuildingUpgrade(DataContainerSetID SetID, string UnitID, double price)
    {
        if (SetID == DataContainerSetID.Building)
        {
            int NowLevel = JsonData[DataContainerSetID.Building.ToString()][UnitID].intValue;
         
            JsonData[SetID.ToString()].SetField(UnitID,NowLevel + 1);
            return true;
        }
        return false;  
    }

    public static bool UsePaymentCount(PaymentType paymentType,double Count)
    {
        if(Count<0)
        {
            if (JsonData[paymentType.ToString()].doubleValue + Count < 0)
                return false;
            else
                JsonData.SetField(paymentType.ToString(), JsonData[paymentType.ToString()].doubleValue + Count);
        }
        else
        {
            JsonData.SetField(paymentType.ToString(), JsonData[paymentType.ToString()].doubleValue + Count);
            
            AddPayment(paymentType, Count);
        }
        
        return true;
    }
    
    public static double GetTotalPaymentCount(PaymentType paymentType)
    {
        if (totalPayments.ContainsKey(paymentType))
        {
            return totalPayments[paymentType];
        }
        return 0;
    }
    
    public static void AddPayment(PaymentType paymentType, double amount)
    {
        if (!totalPayments.ContainsKey(paymentType))
        {
            totalPayments[paymentType] = 0;
        }
        totalPayments[paymentType] += amount;
    }
}