using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTree : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭 감지
        {
            //DetectClick();
        }
    }

    void DetectClick()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            ClickableObject clickableObject = hit.collider.GetComponent<ClickableObject>();

            if (clickableObject != null)
            {
                clickableObject.OnClick();
            }
        }
    }
}
