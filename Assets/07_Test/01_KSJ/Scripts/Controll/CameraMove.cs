using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    //PC��� ���콺�� ȭ���� ���콺�� �ϰ� ������̶�� ��ġ�� x�����θ� �̵�
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
        //ui�� �ƴ� ������ ���콺 Ŭ����
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
        
        //����̹����� ī�޶󺸴� ������ ī�޶� ����̹����� ����� �ʵ���
        if (Background.bounds.size.x < Camera.main.orthographicSize * 2 * _camera.aspect)
        {
            transform.position = new Vector3(Background.transform.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            //ī�޶� ����̹����� ����� �ʵ���
            if (transform.position.x < Background.bounds.min.x + _camera.orthographicSize * _camera.aspect)
            {
                transform.position = new Vector3(Background.bounds.min.x + _camera.orthographicSize * _camera.aspect, transform.position.y, transform.position.z);
            }
            
            if (transform.position.x > Background.bounds.max.x - _camera.orthographicSize * _camera.aspect)
            {
                transform.position = new Vector3(Background.bounds.max.x - _camera.orthographicSize * _camera.aspect, transform.position.y, transform.position.z);
            }
        }
        
        //��Ŭ������ ����
        if (Input.GetMouseButtonDown(1))
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        
    }
    
}
