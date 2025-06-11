using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

[Serializable]
public class PopupUnitView
{
    public GameObject gameObject;
    public TextMeshProUGUI Title;
    public Image Thumbnail;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI PowText;
    public TextMeshProUGUI SpeedText;

    public void SetData(Units UnitData,int level, Color color)
    {
        Title.text = UnitData.Name;
        Thumbnail.sprite = UnitData.Thumbnail;
        Thumbnail.color = color;
        LevelText.text = level.ToString()+"Lv";
        PowText.text = UnitData.Pow.ToString();
        SpeedText.text = UnitData.Speed.ToString();
    }
}

public class UnitUIManager : MonoBehaviour
{
    [Header("UI Á¤º¸ ÀúÀå")]
    public UnitCard PrefabUnitCard;
    public Transform CarryingUnitParent;
    List<UnitCard> CarryingUnit;
    public Transform HarvestingUnitParent;
    List<UnitCard> HarvestingUnit;
    public Transform PickUpUnitParent;
    List<UnitCard> PickUpUnit;


    [Header("ÆË¾÷")]
    public GameObject PopUpGameObject;
    public TextMeshProUGUI Title;
    public PopupUnitView PopupUnitViewNow;
    public PopupUnitView PopupUnitViewNext;
    public TextMeshProUGUI CoinCount;


    DataContainerSetID SetID;
    string UnitID;
    Action PurchaseCallBack;
    void Start()
    {
        CarryingUnit = new List<UnitCard>();
        for (int i = 0; i < DataContainer.Instance[DataContainerSetID.CarryingUnit].Count; i++)
        {
            UnitCard TempUnitCard = Instantiate(PrefabUnitCard, CarryingUnitParent);
            TempUnitCard.init(this,DataContainerSetID.CarryingUnit, DataContainer.Instance[DataContainerSetID.CarryingUnit][i].ID);
            CarryingUnit.Add(TempUnitCard);
        }

        HarvestingUnit = new List<UnitCard>();
        for (int i = 0; i < DataContainer.Instance[DataContainerSetID.HarvestingUnit].Count; i++)
        {
            UnitCard TempUnitCard = Instantiate(PrefabUnitCard, HarvestingUnitParent);
            TempUnitCard.init(this, DataContainerSetID.HarvestingUnit, DataContainer.Instance[DataContainerSetID.HarvestingUnit][i].ID);
            HarvestingUnit.Add(TempUnitCard);
        }

        PickUpUnit = new List<UnitCard>();
        for (int i = 0; i < DataContainer.Instance[DataContainerSetID.PickUpUnit].Count; i++)
        {
            UnitCard TempUnitCard = Instantiate(PrefabUnitCard, PickUpUnitParent);
            TempUnitCard.init(this, DataContainerSetID.PickUpUnit, DataContainer.Instance[DataContainerSetID.PickUpUnit][i].ID);
            PickUpUnit.Add(TempUnitCard);
        }
    }
    public void OpenPopup(DataContainerSetID _SetID, string _UnitID, Action callBack = null)
    {
        SetID = _SetID;
        UnitID = _UnitID;
        PopUpGameObject.SetActive(true);
        PurchaseCallBack = callBack;
        PurchaseCallBack += () => { UpdateData(); };
        UpdateData();
    }
    public void UpdateData()
    {
        int Level = DataManager.GetUnitLevel(SetID, UnitID);
        UnitContaner UnitContaner = (DataContainer.Instance[SetID][UnitID] as UnitContaner);
        if (Level < 0)
        {
            PopupUnitViewNext.gameObject.SetActive(false);
            Title.text = "À¯´Ö ±¸¸Å"; 
            Units UnitData = UnitContaner[0];
            PopupUnitViewNow.SetData(UnitData, 1,Color.black);
            CoinCount.text = UnitData.Price.ToString();
        }
        else if (Level >= UnitContaner.Count - 1)
        {
            PopupUnitViewNext.gameObject.SetActive(false);
            Title.text = "À¯´Ö °­È­";

            Units UnitData = UnitContaner[Level];
            PopupUnitViewNow.SetData(UnitData, Level+1, Color.white);
            CoinCount.text = "MAX";
        }
        else
        {
            PopupUnitViewNext.gameObject.SetActive(true);
            Title.text = "À¯´Ö °­È­";

            Units UnitData = UnitContaner[Level];
            PopupUnitViewNow.SetData(UnitData, Level + 1, Color.white);

            UnitData = UnitContaner[Level+1];
            PopupUnitViewNext.SetData(UnitData, Level + 2, Color.white);
            CoinCount.text = UnitData.Price.ToString();
        }
    }
    
    public void Purchase()
    {
        //ShopManager.UnitPurchase(SetID, UnitID, PurchaseCallBack);
    }
}
