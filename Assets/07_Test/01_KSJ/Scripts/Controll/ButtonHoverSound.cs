using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverSound : MonoBehaviour
{
    //��ư Ȥ�� ��� ������Ʈ�� ���콺�� �ö����� �Ҹ��� ����ϴ� ��ũ��Ʈ
    //��ư�� �� ��ũ��Ʈ�� �߰��ϸ� �ڵ����� �۵���
    
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
