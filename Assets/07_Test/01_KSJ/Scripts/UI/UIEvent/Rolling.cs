using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//오브젝트 활성화시 아래에서 위로 올라가는 애니메이션
public class Rolling : MonoBehaviour
{
    public RectTransform uiElement;

    private void Awake()
    {
        uiElement = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        // Get the height of the UI element
        float uiElementHeight = uiElement.rect.height;

        // Set the initial position below the screen
        uiElement.anchoredPosition = new Vector2(uiElement.anchoredPosition.x, -uiElementHeight);

        // Animate the UI element to move up to its original position
        uiElement.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutBack);
    }

    public void DisAppear()
    {
        // Get the height of the UI element
        float uiElementHeight = uiElement.rect.height;

        // Animate the UI element to move down below the screen
        uiElement.DOAnchorPosY(-uiElementHeight, 0.5f).SetEase(Ease.InBack).onComplete = () => gameObject.SetActive(false);
    }
    
}
    
