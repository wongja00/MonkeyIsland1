using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleNagative : MonoBehaviour
{
    public void ToggleActiveNegative(bool value)
    {
        gameObject.SetActive(!value);
    }
}
