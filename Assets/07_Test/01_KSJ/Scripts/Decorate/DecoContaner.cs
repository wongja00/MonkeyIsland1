using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecoContaner", menuName = "DataContainer/DecoContaner")]
public class DecoContaner : ItemContaner
{
    public List<Decoration> Decos;
    
    public Sprite this[string ID]
    {
        get
        {
            for (int i = 0; i < Decos.Count; i++)
                if (Decos[i].ID == ID)
                    return Decos[i].Thumbnail;
            return null;
        }
    } 
}

[Serializable]
public class Decoration
{
    public string ID;
    public string Name;
    public Sprite Thumbnail;
}