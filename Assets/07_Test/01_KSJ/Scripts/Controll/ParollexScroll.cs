using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//원근감 스크롤
public class ParollexScroll : MonoBehaviour
{
    //카메라
    public Camera mainCamera;
    //하늘(배경)
    public Transform sky;
    //원경
    public Transform far1;
    public Transform far2;
    //중경
    public Transform mid1;
    //레이어별 속도
    public float skySpeed = 0.1f;
    public float farSpeed = 0.2f;
    public float midSpeed = 0.4f;
    
    // 이전 카메라 위치
    private Vector3 previousCameraPosition;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 초기 카메라 위치 저장
        previousCameraPosition = mainCamera.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        // 카메라의 이동량 계산
        Vector3 deltaMovement = mainCamera.transform.position - previousCameraPosition;

        // 각 레이어에 대해 이동량 적용
        sky.position += new Vector3(deltaMovement.x * skySpeed, deltaMovement.y * farSpeed, 0);
        far1.position += new Vector3(deltaMovement.x * farSpeed, deltaMovement.y * farSpeed, 0);
        far2.position += new Vector3(deltaMovement.x * farSpeed, deltaMovement.y * farSpeed, 0);
        mid1.position += new Vector3(deltaMovement.x * midSpeed, deltaMovement.y * midSpeed, 0);

        // 현재 카메라 위치를 이전 위치로 업데이트
        previousCameraPosition = mainCamera.transform.position;
    }
}
