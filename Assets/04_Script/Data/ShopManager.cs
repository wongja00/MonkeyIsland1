using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class ShopManager
{
    public static Action<DataContainerSetID, string, int> OnUnitPurchase;
    public static Action<DataContainerSetID, int> OnStoragePurchase;
    public static Action<DataContainerSetID, int> OnProductorPurchase;
    public static Action<DataContainerSetID, int> OnMarketPurchase;
    public static Action<DataContainerSetID, string, int> OnBuildingPurchase;
    public static Action OnGoldUpdate;
    public static Action<PaymentType> OnPayUpdate;
    
    public static bool UnitPurchase(DataContainerSetID SetID, string UnitID, int Level = 1, Action callBack = null, PaymentType paymentType = PaymentType.Gold, double price = 0d)
    {
        if (SetID == DataContainerSetID.CarryingUnit || SetID == DataContainerSetID.HarvestingUnit || SetID == DataContainerSetID.PickUpUnit)
        {
            if (!UsePaymentCount(paymentType, -price))
            {
                return false;
            }
            
            callBack?.Invoke();
            DataManager.UnitUpgrade(SetID, UnitID, price);
            OnUnitPurchase?.Invoke(SetID,UnitID, Level + 1);
            

            return true;
        }
        return false;
    }
    
    public static bool BuildingPurchase(DataContainerSetID SetID, string BuildingID, double price, int EnforceValue, PaymentType paymentType, Action callBack = null)
    {
        if (SetID == DataContainerSetID.Building)
        {
            int Level = EnforceValue;

            if(Level <= -1)
            {
                Level = 0;
            }

            if(!UsePaymentCount(paymentType, -price))
            {
                return false;
            }
            
            if (!DataManager.BuildingUpgrade(SetID, BuildingID, price))
            {
                return false;
            }
            
            OnBuildingPurchase?.Invoke(SetID, BuildingID, Level + 1);
            callBack?.Invoke();
            return true;
        }
        return false;
    }   

    public static bool UsePaymentCount(PaymentType paymentType, double Count)
    {
        //??? ???? ???? ???? ????? ??????? ???? ??? ????
        bool temp = DataManager.UsePaymentCount(paymentType, Count);
        OnGoldUpdate?.Invoke();
        
        if(Count != 0)
            OnPayUpdate?.Invoke(paymentType);
        
        return temp;
    }
    public static double GetPaymentCount(PaymentType paymentType)
    {
        return DataManager.GetPaymentCount(paymentType);
    }
}
