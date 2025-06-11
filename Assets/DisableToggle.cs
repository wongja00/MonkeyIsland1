using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableToggle : MonoBehaviour
{
    public Toggle toggle;

    private void Start()
    {
        Toggle();
    }

    public void Toggle()
    {
        toggle.targetGraphic.color = toggle.isOn ? Color.white : Color.gray;
    }
}
