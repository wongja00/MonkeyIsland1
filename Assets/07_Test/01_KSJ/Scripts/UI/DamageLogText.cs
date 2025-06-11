using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageLogText : MonoBehaviour
{
    public TextMeshProUGUI damageText;

    public void SetText(string _damage)
    {
        damageText.text = _damage;
    }
    
    public void SetColor(Color _color)
    {
        damageText.color = _color;
    }
    
    public void SetFontSize(int _size)
    {
        damageText.fontSize = _size;
    }
    
    //�ö󰡸鼭 ������� �α�
    public void MoveUpAndFadeOut(float _speed)
    {
        // �ؽ�Ʈ�� ���� �̵��ϴ� �ִϸ��̼�
        transform.DOMoveY(transform.position.y + 1, _speed);

        // �ؽ�Ʈ�� ���� ���������� �ִϸ��̼�
        damageText.DOFade(0, _speed).OnComplete(() => Destroy(gameObject));
        
    }
    
}
