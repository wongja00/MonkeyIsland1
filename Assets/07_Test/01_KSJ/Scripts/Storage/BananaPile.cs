using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaPile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    private StorageChest storageChest;
    
    [SerializeField]
    private Sprite[] sprites = new Sprite[4];
    
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        storageChest = GetComponent<StorageChest>();

        ShopManager.OnGoldUpdate += BananaPileUp;
    }

    private void BananaPileUp()
    {
        if (storageChest.BananaCount <= 1000)
        {
            spriteRenderer.sprite = sprites[0];
        }
        else if (storageChest.BananaCount <= 5000)
        {
            spriteRenderer.sprite = sprites[1];
        }
        else if (storageChest.BananaCount <= 10000)
        {
            spriteRenderer.sprite = sprites[2];
        }
        else
        {
            spriteRenderer.sprite = sprites[3];
        }
        
    }
}
