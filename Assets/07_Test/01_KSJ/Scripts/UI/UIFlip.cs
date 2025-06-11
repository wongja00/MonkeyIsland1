using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class UIFlip : MonoBehaviour
{
    float targetPosition = 0;

    [SerializeField]
    TextMeshProUGUI Arrowtext;
    
    
    bool isFlipped = false;
    
    private void Start()
    {
        targetPosition = -transform.parent.GetComponent<RectTransform>().sizeDelta.x;
        
        isFlipped = false;
    }
    
    //�θ������Ʈ�� �θ������ ��ŭ UI �������� �̵�
    public void Flip()
    {
        if(isFlipped)
        {
            transform.parent.DOMoveX(0, 0.5f).OnComplete(() =>
            {
                isFlipped = false;
                Arrowtext.text = "<-";
            });
            return;
        }
        
        transform.parent.DOMoveX(targetPosition, 0.5f).OnComplete(() =>
        {
            isFlipped = true;
            Arrowtext.text = "->";
        });
    }
}
