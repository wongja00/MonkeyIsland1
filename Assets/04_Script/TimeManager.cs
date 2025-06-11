using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public DayCycleHandler dayCycleHandler;

    [SerializeField]
    float DayTimeLegth;
    float DeltaTime;

    private void Start()
    {
        DeltaTime = DayTimeLegth / 2;
    }

    void Update()
    {
        DeltaTime += Time.deltaTime;
        dayCycleHandler.UpdateLight(GetTimeNormal());
    }
    public float GetTimeNormal()
    {
        return (DeltaTime % DayTimeLegth) / DayTimeLegth;
    }
}
