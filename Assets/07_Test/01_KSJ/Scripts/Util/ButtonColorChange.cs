using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChange : MonoBehaviour
{
    public Color FirstColor;
    public Color SecondColor;
    public Color ThirdColor;

    public Image btnImage;
    
    public void ChangeFirstColor()
    {
        btnImage.color = FirstColor;
    }
    
    public void ChangeSecondColor()
    {
        btnImage.color = SecondColor;
    }
    
    public void ChangeThirdColor()
    {
        btnImage.color = ThirdColor;
    }
    
}
