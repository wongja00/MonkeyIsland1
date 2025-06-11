using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCanvasResolution : MonoBehaviour
{
    public Canvas uiCanvas; // UI Canvas�� �����ϴ� �ʵ带 �߰�
    public Camera mainCamera; // Main Camera�� �����ϴ� �ʵ带 �߰�
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
        // Camera�� rect �Ӽ��� �����ɴϴ�.
        Rect cameraRect = mainCamera.rect;

        // UI Canvas�� ũ�⸦ Camera�� rect�� �°� �����մϴ�.
        rectTransform.sizeDelta = new Vector2(cameraRect.width * Screen.width, cameraRect.height * Screen.height);

        // UI Canvas�� ��ġ�� ���͹ڽ��� �߾ӿ� ����ϴ�.
        rectTransform.anchoredPosition = new Vector2(cameraRect.x * Screen.width, cameraRect.y * Screen.height);
    }
}
