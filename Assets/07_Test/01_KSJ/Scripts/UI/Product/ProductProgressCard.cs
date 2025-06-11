using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductProgressCard : MonoBehaviour
{
    public Image Thumbnail;
    public Image ProgressBar;

    public int time;
    public int price;

    public string _name;
    
    public void UpdateData(float progress)
    {
        ProgressBar.fillAmount = progress;
    }
}
