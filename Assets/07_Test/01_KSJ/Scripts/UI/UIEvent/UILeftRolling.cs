using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

//���� �Ѹ� �̺�Ʈ
public class UILeftRolling : MonoBehaviour
{
    //�ڽ� ������Ʈ�� RectTransform ������Ʈ
    public RectTransform uiElement;
    
    public List<RectTransform> uiElements;
    
    public Vector2 uiOriginalPosition;
    
    //���
    public Toggle toggle;

    void Start()
    {
        uiOriginalPosition = uiElement.anchoredPosition;
    }
    
    
    public void Rolling(bool isRolling)
    {
        if (isRolling)
        {
            for(int i = 0; i < uiElements.Count; i++)
            {
                uiElements[i].DOAnchorPosX(uiOriginalPosition.x - ((i+1)  * 200), 0.5f).SetEase(Ease.OutBack);
            }
        }
        else
        {
            for(int i = 0; i < uiElements.Count; i++)
            {
                uiElements[i].DOAnchorPosX(uiOriginalPosition.x, 0.5f).SetEase(Ease.InBack);
            }
        }
    }
}
