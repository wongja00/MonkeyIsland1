using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//유닛 강화 버튼 클릭시 강화할 수치를 정해주는 스크립트
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
        //강화할 수치를 정해주는 함수
        enforceValue = value;
    }

}
