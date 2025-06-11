using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemSetContaner ", menuName = "DataContainer/ItemSetContaner ")]
public class ItemSetContaner : ScriptableObject
{
    public List<ItemContaner> ItemContaner;
    public int Count { get { return ItemContaner.Count; } }
    public ItemContaner this[string index]
    {
        get
        {
            for (int i = 0; i < ItemContaner.Count; i++)
                if (ItemContaner[i].ID == index)
                    return ItemContaner[i];
            return null;
        }
    }
    public ItemContaner this[int index]
    {
        get
        {
            return ItemContaner[index];
        }
    }
    public int GetDataNum(string index)
    {
        for (int i = 0; i < ItemContaner.Count; i++)
            if (ItemContaner[i].ID == index)
                return i;
        return -1;
    }
}