using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinkApplier : MonoBehaviour
{
    void Start()
    {
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => ButtonWink.Wink(button.gameObject));
        }
    }
}
