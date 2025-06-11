using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventObjectContaner", menuName = "DataContainer/EventObjectContaner")]
public class EventObjectContainer : ItemContaner
{
    public List<EventObject> EventObjectData;
    
    public int Count { get { return EventObjectData.Count; } }
    
    public EventObject this[int index]
    {
        get { return EventObjectData[index]; }
    }
    
    public EventObject this[string Name]
    {
        get { return EventObjectData.Find(x => x.Name == Name); }
    }
}


[Serializable]
public class EventObject
{
    public string Name;
    public Sprite Thumbnail;
}
