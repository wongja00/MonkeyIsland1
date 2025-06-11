using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//���� ��ȭ ��ư Ŭ���� ��ȭ�� ��ġ�� �����ִ� ��ũ��Ʈ
public class UnitEnforceValue : MonoBehaviour
{
    [SerializeField]
    private Toggle btn1;
    
    [SerializeField]
    private Toggle btn10;
    
    [SerializeField]
    private Toggle btn100;
    
    [SerializeField]
    private Toggle btnMax;

    public static int enforceValue = 1;
    
    private void Awake()
    {
        Init();
    }
    
    void OnDisable()
    {
        btn1.isOn = true;
        EnforceValue(1);
    }

    private void Init()
    {
        btn1.onValueChanged.AddListener((value) => { if (value) { EnforceValue(1); } });
        btn10.onValueChanged.AddListener((value) => { if (value) { EnforceValue(10); } });
        btn100.onValueChanged.AddListener((value) => { if (value) { EnforceValue(100); } });
        btnMax.onValueChanged.AddListener((value) => { if (value) { EnforceValue(999); } });
    }

    private void EnforceValue(int value)
    {
        //��ȭ�� ��ġ�� �����ִ� �Լ�
        enforceValue = value;
    }

}
