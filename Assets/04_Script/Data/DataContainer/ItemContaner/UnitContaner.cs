using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitContaner", menuName = "DataContainer/UnitContaner")]
public class UnitContaner : ItemContaner
{
    public int code;
    public uint MonkeyIndex;
    public uint MaxHire;
    public List<Units> Units;
    public int Count { get { return Units.Count; } }
    public Units this[int Level]
    {
        get { return Units[Level];  }
    }
}

[Serializable]
public class Units
{
    public Sprite Thumbnail;
    public string Name;
    public uint Pow;
    public float Speed;
    public uint Price;
    public uint PowState;
    public uint SpeedState;
}