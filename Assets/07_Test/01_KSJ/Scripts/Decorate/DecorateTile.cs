using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//타일
public class DecorateTile : MonoBehaviour
{
    public bool isTile = false;
    public DecorateType tileType; 
    
    //타일 이미지
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
