using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DataContainerSetID
{
    HarvestingUnit,
    PickUpUnit,
    CarryingUnit,
    TempStorage,
    Storage,
    Building,
    Skill,
    Deco,
    Product,
    JuiceProductor,
    CookProductor,
    DessertProductor,
    Market
}

[CreateAssetMenu(fileName = "DataContainer", menuName = "DataContainer/DataContainer")]
public class DataContainer : SingletonScriptableObject<DataContainer>
{
    //???? ???
    public ItemSetContaner HarvestingUnitContainer;
    public ItemSetContaner PickUpUnitContainer;
    public ItemSetContaner CarryingUnitContainer;

    //?????? ???
    public ItemSetContaner TempStorageContainer;
    public ItemSetContaner NomalStorageContainer;

    //??? ???
    public ItemSetContaner buildingContainer;
    
    //??? ???
    public ItemSetContaner SkillContainer;
    
    public ItemSetContaner decoContainer;
    
    //??? ???
    public ItemSetContaner productContainer;
    

    //??? ???
    public ItemSetContaner JuiceProductorContainer;
    public ItemSetContaner CookProductorContainer;
    public ItemSetContaner DessertProductorContainer;
    public ItemSetContaner MarketContainer;
    
    public ItemSetContaner this[DataContainerSetID _Type]
    {
        get
        {
            switch (_Type)
            {
                case DataContainerSetID.HarvestingUnit:
                        return HarvestingUnitContainer;
                case DataContainerSetID.PickUpUnit:
                        return PickUpUnitContainer;
                case DataContainerSetID.CarryingUnit:
                        return CarryingUnitContainer;
                case DataContainerSetID.Storage:
                        return NomalStorageContainer;
                case DataContainerSetID.TempStorage:
                        return TempStorageContainer;
                case DataContainerSetID.Building:
                        return buildingContainer;
                case DataContainerSetID.Skill:
                        return SkillContainer;
                case DataContainerSetID.Deco:
                        return decoContainer;
                case DataContainerSetID.Product:
                        return productContainer;
                case DataContainerSetID.JuiceProductor:
                        return JuiceProductorContainer;
                case DataContainerSetID.CookProductor:
                        return CookProductorContainer;
                case DataContainerSetID.DessertProductor:
                        return DessertProductorContainer;
                case DataContainerSetID.Market:
                        return MarketContainer;
                default:
                        return new ItemSetContaner();
            }

        }
    }
}