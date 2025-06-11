using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UnitCard : MonoBehaviour
{
    UnitUIManager unitUIManager;

    //UI ¿¬°á ºÎ
    public TextMeshProUGUI Title;
    public TextMeshProUGUI LevelText;
    public Image Thumbnail;
    public Image Pow;
    public Image Speed;

    DataContainerSetID SetID;
    string UnitID;

    public void init(UnitUIManager _unitUIManager, DataContainerSetID _SetID, string _UnitID)
    {
        unitUIManager = _unitUIManager;
        SetID = _SetID;
        UnitID = _UnitID;
        UpdateData();
    }
    public void UpdateData()
    {
        int Level = DataManager.GetUnitLevel(SetID, UnitID);
        UnitContaner UnitContaner = (DataContainer.Instance[SetID][UnitID] as UnitContaner);
        if (Level < 0)
        {
            Units UnitData = UnitContaner[0];
            Title.text = UnitData.Name;
            Thumbnail.sprite = UnitData.Thumbnail;
            Thumbnail.color = Color.black;
            LevelText.text = "";
            Pow.fillAmount = UnitData.PowState/5f;
            Speed.fillAmount = UnitData.SpeedState / 5f;
        }
        else if (UnitContaner.Count > Level)
        {
            Units UnitData = UnitContaner[Level];
            Title.text = UnitData.Name;
            Thumbnail.sprite = UnitData.Thumbnail;
            Thumbnail.color = Color.white;
            LevelText.text = (Level + 1).ToString();
            Pow.fillAmount = UnitData.PowState / 5f;
            Speed.fillAmount = UnitData.SpeedState / 5f;
        }
    }
    public void Purchase()
    {
        unitUIManager.OpenPopup(SetID, UnitID, () => UpdateData());
    }
}
