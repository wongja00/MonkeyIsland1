using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillContaner", menuName = "DataContainer/SkillContaner")]
public class SkillContaner : ItemContaner
{
    public List<Skills> Skilldata;

    public int Count { get { return Skilldata.Count; } }
    
    public Skills this[int Index]
    {
        get { return Skilldata[Index];  }
    }

    public Skills this[string ID]
    {
        get { return Skilldata.Find(x => x.ID == ID); }
    }
    
    
}

[Serializable]
public class Skills
{
    public string Name;
    public string ID;
}