using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMesh;

    [SerializeField]
    private Color color;
    
    public void ChangeColor(bool isToggle)
    {
        if (isToggle)
            textMesh.color = Color.black;
        else
            textMesh.color = Color.white;
    }
    
    public void ChangeColor()
    {
        textMesh.color = color;
    }
}
