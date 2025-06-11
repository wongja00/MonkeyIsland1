using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorateObject : MonoBehaviour, DecorateObjectInterface
{
    public DecorateBuff decorateBuff { get; set; }
    public DecorateType decorateType;
    public DecorateTile decorateTile;
    public SpriteRenderer spriteRenderer;
    public Sprite sprite;
    public bool isDecorate { get; set; }

    public void Decorate()
    {   
        decorateTile.Set();
    }

    public void Remove()
    {
        decorateTile.Remove();
        spriteRenderer.sprite = null;
    }

    public bool CheckDecorate()
    {
        return !decorateTile.isTile;
    }
    
}
