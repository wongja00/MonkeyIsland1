using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ٳ��� �α� ���� ��ũ��Ʈ
public class DamageLog : MonoBehaviour
{
    //�ٳ��� �α� ������
    public GameObject damageLogPrefab;
    
    [SerializeField]
    private Transform damageLogParent;
    
    //�ٳ��� �α׸� ���� �Լ�
    public void ShowDamageLog(string _damage)
    {
        GameObject damageLog = Instantiate(damageLogPrefab, damageLogParent);
        DamageLogText textLog = damageLog.GetComponent<DamageLogText>();
        textLog.SetText(_damage);
        textLog.MoveUpAndFadeOut(1.0f);
    }
    
}
