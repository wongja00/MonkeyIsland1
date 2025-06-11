using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    public float TimeScale  = 1f;

    private void Update()
    {
        Time.timeScale = TimeScale;
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
        TimeScale = timeScale;
    }
    
    public void ResetTimeScale()
    {
        Time.timeScale = 1f;
        TimeScale = 1f;
    }
    
    public void PauseTime()
    {
        Time.timeScale = 0f;
        TimeScale = 0f;
    }
    
    public void ResumeTime()
    {
        Time.timeScale = 1f;
        TimeScale = 1f;
    }
    
}
