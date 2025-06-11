using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWink : MonoBehaviour
{
    private static bool isAnimating = false;
 
    public static void Wink(GameObject btn)
    {
        if (isAnimating) return;
        
        Vector3 scale = btn.transform.localScale;
        
        isAnimating = true;
        Sequence sequence = DOTween.Sequence();

        sequence.Append(btn.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.1f));
        sequence.Append(btn.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f));
        sequence.Append(btn.transform.DOScale(scale, 0.1f));

        sequence.OnComplete(() => isAnimating = false);
    }
}
