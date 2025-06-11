using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundKine 
{
    BGM,
    SFX,
    Env
}

[CreateAssetMenu(fileName = "SoundContaner", menuName = "DataContainer/SoundContaner")]
public class SoundContaner : ScriptableObject
{
    public List<SoundInfo> SoundList;
}

[Serializable]
public class SoundInfo
{
    public int Code;
    public string Name;
    public string Description;
    public float Delay;
    public bool isLoop;
    public SoundKine SoundKind;
    public AudioClip Sound;
}
