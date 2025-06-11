using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StorageContaner", menuName = "DataContainer/StorageContaner")]
public class StorageContaner : ItemContaner
{
    public List<Storages> Storages;

    public int Count { get { return Storages.Count; } }
    public Storages this[int Level]
    {
        get { return Storages[Level];  }
    }
    
}

[Serializable]
public class Storages
{
    public string Name;
    public Sprite Thumbnail;
    public uint MaxStorage;
    public uint Price;
}
