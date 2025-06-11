using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

//정리, 운반 원숭이 이동 로직(사실 좌우 이동밖에 없음 ㅎㅎ)
public class MonkeyCarryMove : MonoBehaviour
{
    public void MoveByTime(Transform Storage, float time, Action updateCallback)
    {
        transform.DOMoveX(Storage.transform.position.x, time).SetEase(Ease.Linear).onUpdate = () =>
        {
            updateCallback();
        };
    }
    
    public void MoveBySpeed(Transform Storage, float speed, Action updateCallback)
    {
        float distance = Mathf.Abs(transform.position.x - Storage.transform.position.x);
        float time = distance / speed;
        
        transform.DOMoveX(Storage.transform.position.x, time).SetEase(Ease.Linear).onUpdate = () =>
        {
            updateCallback();
        };
    }
    
    public void FlipSprite(Transform Storage, SpriteRenderer spriteRenderer, bool Reverse = false)
    {
        if (Reverse)
        {
            spriteRenderer.flipX = transform.position.x > Storage.transform.position.x;
        }
        else
        {
            spriteRenderer.flipX = transform.position.x < Storage.transform.position.x;
        }
    }
}
