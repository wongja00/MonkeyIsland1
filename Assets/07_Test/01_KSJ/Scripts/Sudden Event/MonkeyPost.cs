using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MonkeyPost : MonoBehaviour, ClickableObject, SuddenEventObject
{
    public bool isEvent { get; set; }

    public float eventTime = 10;
    public float speed = 1;
    
    Vector3 originPos;
    
    [SerializeField]
    private Camera mainCamera;

    private float leftBoundary;
    private float rightBoundary;
    
    private Coroutine coroutine;
    
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    public float outTime = 60;
    
    private void Start()
    {
        isEvent = false;
        mainCamera = Camera.main;
        
        originPos = transform.position;
    }
    
    public void OnClick()
    {
        MonkeyPostEventManager.instance.Popup(this);
    }

    public void OnEvent()
    {
        isEvent = true;
        
        gameObject.SetActive(true);
        
        transform.position = originPos;
        
        coroutine = StartCoroutine(Move());
    }
    
    public void EventOut()
    {
        StopCoroutine(coroutine);
        isEvent = false;
        gameObject.SetActive(false);
    }
    
    private IEnumerator Move()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        bool movingRight = true;
        
        while (outTime > elapsedTime)
        {
            leftBoundary = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
            rightBoundary = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
            
            float upDown = (movingRight ? Mathf.Sin(elapsedTime * speed) : Mathf.Cos(elapsedTime * speed)) * 0.5f;
            float newX = startPosition.x + (movingRight ? elapsedTime * speed : -elapsedTime * speed);
            spriteRenderer.flipX = movingRight;
            
            // 좌우 경계를 벗어나지 않도록 위치 제한 및 방향 전환
            if (newX > rightBoundary)
            {
                newX = rightBoundary;
                movingRight = false;
                elapsedTime = 0f; // 방향 전환 시 시간 초기화
                startPosition = new Vector3(newX, startPosition.y, startPosition.z);
            }
            else if (newX < leftBoundary)
            {
                newX = leftBoundary;
                movingRight = true;
                elapsedTime = 0f; // 방향 전환 시 시간 초기화
                startPosition = new Vector3(newX, startPosition.y, startPosition.z);
            }

            transform.position = new Vector3(newX, startPosition.y + upDown, startPosition.z);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        MonkeyPostEventManager.instance.EventOut();
    }
}
