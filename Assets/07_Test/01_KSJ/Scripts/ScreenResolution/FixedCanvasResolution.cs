using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCanvasResolution : MonoBehaviour
{
    public Canvas uiCanvas; // UI Canvas를 참조하는 필드를 추가
    public Camera mainCamera; // Main Camera를 참조하는 필드를 추가
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = uiCanvas.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustUICanvas();
    }

    private void AdjustUICanvas()
    {
        // Camera의 rect 속성을 가져옵니다.
        Rect cameraRect = mainCamera.rect;

        // UI Canvas의 크기를 Camera의 rect에 맞게 조정합니다.
        rectTransform.sizeDelta = new Vector2(cameraRect.width * Screen.width, cameraRect.height * Screen.height);

        // UI Canvas의 위치를 레터박스의 중앙에 맞춥니다.
        rectTransform.anchoredPosition = new Vector2(cameraRect.x * Screen.width, cameraRect.y * Screen.height);
    }
}
