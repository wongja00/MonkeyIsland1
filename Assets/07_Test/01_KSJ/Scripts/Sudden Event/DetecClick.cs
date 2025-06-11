using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetecClick : MonoBehaviour
{
    Camera mainCamera;
    
    ClickableObject clickableObject;
    
    [SerializeField]
    BoxCollider2D boxCollider2D;
    
    void Start()
    {
        mainCamera = Camera.main;
        clickableObject = GetComponent<ClickableObject>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭 감지
        {
            DetectClick();
        }
    }
    
    void DetectClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePosition);

        if (hit == boxCollider2D && hit != null && clickableObject != null)
        {
            clickableObject.OnClick();
        }
    }
}
