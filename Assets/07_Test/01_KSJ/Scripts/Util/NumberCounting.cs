using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberCounting : MonoBehaviour
{
    public TextMeshProUGUI countLabel;
    
        IEnumerator Count(float target, float current)    
        {
            float duration = 0.5f; // ī���ÿ� �ɸ��� �ð� ����. 
            float offset = (target - current) / duration;    
    
            while (current < target)
            {
                current += offset * Time.deltaTime;    
                countLabel.text = ((int)current).ToString();    
                yield return null;
            }
            current = target;    
            countLabel.text = ((int)current).ToString();
        }
}
