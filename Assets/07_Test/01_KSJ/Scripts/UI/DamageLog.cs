using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//바나나 로그 띄우는 스크립트
public class DamageLog : MonoBehaviour
{
    //바나나 로그 프리팹
    public GameObject damageLogPrefab;
    
    [SerializeField]
    private Transform damageLogParent;
    
    //바나나 로그를 띄우는 함수
    public void ShowDamageLog(string _damage)
    {
        GameObject damageLog = Instantiate(damageLogPrefab, damageLogParent);
        DamageLogText textLog = damageLog.GetComponent<DamageLogText>();
        textLog.SetText(_damage);
        textLog.MoveUpAndFadeOut(1.0f);
    }
    
}
