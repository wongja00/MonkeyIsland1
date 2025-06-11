using System.Linq;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    void Start()
    {
        CombineSprites();
    }

    void CombineSprites()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        if (spriteRenderers.Length == 0) return;

        GameObject combinedObject = new GameObject("CombinedSprite");
        combinedObject.transform.SetParent(transform);
        SpriteRenderer combinedRenderer = combinedObject.AddComponent<SpriteRenderer>();

        int width = 0;
        int height = 0;
        
        int left = 0;
        int right = 0;
        
        foreach (var sprite in spriteRenderers)
        {
            left = Mathf.Min(left, Mathf.RoundToInt(sprite.transform.position.x*100 ));
            
            right = Mathf.Max(right, Mathf.RoundToInt(sprite.transform.position.x*100));
            height = Mathf.Max(height, Mathf.RoundToInt(sprite.sprite.rect.height));
        }
        width = (right - left);
        
        Debug.Log(left);
        Debug.Log(right);
        Debug.Log(width);
        
        Texture2D finalTexture = new Texture2D(width, height);
        finalTexture.filterMode = FilterMode.Point;
        
        Color[] transparentPixels = new Color[width * height];
        for (int i = 0; i < transparentPixels.Length; i++)
        {
            transparentPixels[i] = Color.clear;
        }
        finalTexture.SetPixels(transparentPixels);
        
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            Texture2D spriteTexture = spriteRenderers[i].sprite.texture;

            Rect spriteRect = spriteRenderers[i].sprite.rect;
            Color[] spritePixels = spriteTexture.GetPixels((int)spriteRect.x, (int)spriteRect.y, (int)spriteRect.width, (int)spriteRect.height);
            finalTexture.SetPixels((int)spriteRenderers[i].transform.position.x, (int)spriteRenderers[i].transform.position.y, (int)spriteRect.width, (int)spriteRect.height, spritePixels);

        }
        finalTexture.Apply();

        Sprite combinedSprite = Sprite.Create(finalTexture, new Rect(0, 0, finalTexture.width, finalTexture.height), new Vector2(0.5f, 0.5f));
        combinedRenderer.sprite = combinedSprite;

        foreach (var spriteRenderer in spriteRenderers)
        {
            Destroy(spriteRenderer.gameObject);
        }
    }
}