using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

//좌측 롤링 이벤트
public class UILeftRolling : MonoBehaviour
{
    //자식 오브젝트의 RectTransform 컴포넌트
    public RectTransform uiElement;
    
    public List<RectTransform> uiElements;
    
    public Vector2 uiOriginalPosition;
    
    //토글
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
