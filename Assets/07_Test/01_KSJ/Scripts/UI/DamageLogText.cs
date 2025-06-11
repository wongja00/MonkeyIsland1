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
    
    //올라가면서 사라지는 로그
    public void MoveUpAndFadeOut(float _speed)
    {
        // 텍스트가 위로 이동하는 애니메이션
        transform.DOMoveY(transform.position.y + 1, _speed);

        // 텍스트가 점점 투명해지는 애니메이션
        damageText.DOFade(0, _speed).OnComplete(() => Destroy(gameObject));
        
    }
    
}
