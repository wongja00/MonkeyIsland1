using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverSound : MonoBehaviour
{
    //버튼 혹은 토글 오브젝트에 마우스가 올라갔을때 소리를 재생하는 스크립트
    //버튼에 이 스크립트를 추가하면 자동으로 작동함
    
    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;
    
    public void OnHoverSound()
    {
        audioSource.PlayOneShot(hoverSound);
    }
    public void OnClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
