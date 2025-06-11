using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SuddenEventObject 
{
    public bool isEvent { get; set; }
    public void OnEvent();
    public void EventOut();
}
