using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ÿ��
public class DecorateTile : MonoBehaviour
{
    public bool isTile = false;
    public DecorateType tileType; 
    
    //Ÿ�� �̹���
    public SpriteRenderer spriteRenderer;
    public void Set()
    {
        isTile = true;
    }

    public void Remove()
    {
        isTile = false;
    }
}
