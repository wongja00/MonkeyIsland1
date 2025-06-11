using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ٰ� ��ũ��
public class ParollexScroll : MonoBehaviour
{
    //ī�޶�
    public Camera mainCamera;
    //�ϴ�(���)
    public Transform sky;
    //����
    public Transform far1;
    public Transform far2;
    //�߰�
    public Transform mid1;
    //���̾ �ӵ�
    public float skySpeed = 0.1f;
    public float farSpeed = 0.2f;
    public float midSpeed = 0.4f;
    
    // ���� ī�޶� ��ġ
    private Vector3 previousCameraPosition;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        // �ʱ� ī�޶� ��ġ ����
        previousCameraPosition = mainCamera.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        // ī�޶��� �̵��� ���
        Vector3 deltaMovement = mainCamera.transform.position - previousCameraPosition;

        // �� ���̾ ���� �̵��� ����
        sky.position += new Vector3(deltaMovement.x * skySpeed, deltaMovement.y * farSpeed, 0);
        far1.position += new Vector3(deltaMovement.x * farSpeed, deltaMovement.y * farSpeed, 0);
        far2.position += new Vector3(deltaMovement.x * farSpeed, deltaMovement.y * farSpeed, 0);
        mid1.position += new Vector3(deltaMovement.x * midSpeed, deltaMovement.y * midSpeed, 0);

        // ���� ī�޶� ��ġ�� ���� ��ġ�� ������Ʈ
        previousCameraPosition = mainCamera.transform.position;
    }
}
