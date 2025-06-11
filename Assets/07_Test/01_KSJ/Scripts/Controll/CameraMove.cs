using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    //PC라면 마우스로 화면을 마우스로 하고 모바일이라면 터치로 x축으로만 이동
    private Vector3 startMousePos;
    
    private Vector3 startCameraPos;
    
    public float Sensitivity = 0.1f;
    
    private bool isDrag = false;
    
    [SerializeField] 
    SpriteRenderer Background;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        //ui가 아닌 곳에서 마우스 클릭시
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            startMousePos = Input.mousePosition;
            
            startCameraPos = transform.position;
            
            isDrag = true;
        }
        
        if (Input.GetMouseButton(0) && isDrag)
        {
            Vector3 diff = (Input.mousePosition - startMousePos) * Sensitivity;
            
            transform.position = startCameraPos + new Vector3(-diff.x, 0, 0);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
        }
        
        //배경이미지가 카메라보다 작으면 카메라가 배경이미지를 벗어나지 않도록
        if (Background.bounds.size.x < Camera.main.orthographicSize * 2 * _camera.aspect)
        {
            transform.position = new Vector3(Background.transform.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            //카메라가 배경이미지를 벗어나지 않도록
            if (transform.position.x < Background.bounds.min.x + _camera.orthographicSize * _camera.aspect)
            {
                transform.position = new Vector3(Background.bounds.min.x + _camera.orthographicSize * _camera.aspect, transform.position.y, transform.position.z);
            }
            
            if (transform.position.x > Background.bounds.max.x - _camera.orthographicSize * _camera.aspect)
            {
                transform.position = new Vector3(Background.bounds.max.x - _camera.orthographicSize * _camera.aspect, transform.position.y, transform.position.z);
            }
        }
        
        //우클릭으로 영점
        if (Input.GetMouseButtonDown(1))
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        
    }
    
}
