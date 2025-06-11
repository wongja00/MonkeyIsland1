using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingContaner", menuName = "DataContainer/BuildingContaner")]
public class BuildingContaner : ItemContaner
{
    public List<Buildings> Buildingdata;

    public int Count { get { return Buildingdata.Count; } }
    public Buildings this[int Level]
    {
        get { return Buildingdata[Level];  }
    }

    public Buildings this[string ID]
    {
        get { return Buildingdata.Find(x => x.ID == ID); }
    }
    
    
}

[Serializable]
public class Buildings
{
    public string Name;
    public string ID;
    public Sprite Thumbnail;
}