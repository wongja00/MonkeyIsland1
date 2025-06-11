using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StorageProgressBar : MonoBehaviour
{
    // 게이지 현재 값
    private float currentGauge;
    
    //게이지 최대 값
    private float maxGauge = 100;

    [SerializeField]
    private Image FillRectImage;

    public Gradient TestGradient;
    
    public void GaugeInit(float curgauge, float maxGauge)
    {
        // 게이지 최대값 설정
        this.maxGauge = maxGauge;
        
        // 게이지 초기화
        currentGauge = curgauge;
        
        // 게이지 현재값 설정
        FillRectImage.fillAmount = currentGauge / maxGauge;
        
        SetGauge(currentGauge);
    }
    
    //최대 게이지 갱신
    public void SetMaxGauge(float value)
    {
        maxGauge = value;
    }
    
    // 게이지 증가
    public void SetGauge(float value)
    {
        // 게이지 현재값 증가
        currentGauge = value;
        
        // 게이지 최대값을 넘어가면 최대값, 최소값을 넘어가면 최소값으로 설정
        currentGauge = Math.Clamp(currentGauge, 0, maxGauge);
        
        FillRectImage.fillAmount = currentGauge / maxGauge;
        
        // 게이지 현재값 70%까지는 초록색 90%까지는 노란색 100%까지는 빨간색
        FillRectImage.color = TestGradient.Evaluate(currentGauge / maxGauge);
    }
    
    // 게이지 현재값 반환
    public float GetCurrentGauge()
    {
        return currentGauge;
    }
    
}
