using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//타일맵
public class TileMap : MonoBehaviour
{
    public static TileMap instance;

    public double TileSize = 1.0;
    
    public List<DecorateTile> TileData;

    public Transform TileStartPos;
    
    public Transform TileEndPos;
    
    public DecorateTile tilePreset;

    public Transform tileParent;

    public bool isTileMode = false;
    
    //타일맵
    public Tilemap tilemap;
    
    private void Awake()
    {
        instance = this;
    }
    
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        TileData = GetObjectsOnTiles();
    }

    public DecorateTile GetTile(DecorateType tileType)
    {
        return TileData.Find(x => x.tileType == tileType);
    }

    //타일 타입에 따라 색 바꿈
    public void CheckTilePlace(DecorateType type)
    {
        for (int i = 0; i < TileData.Count; i++)
        {
            if (TileData[i].tileType == type)
            {
                TileData[i].spriteRenderer.color = new Color(0, 1, 0, 0.2f);
            }
            else
            {
                TileData[i].spriteRenderer.color = new Color(1, 0, 0, 0.2f);
            }
        }
    }

    public void DecorateMode(bool isOn)
    {
        CheckTilePlace(DecorateManager.instance.currentDecorateObject.decorateType);
        
        // tileParent.gameObject.SetActive(isOn);
        tilemap.gameObject.SetActive(isOn);
        isTileMode = true;
    }
    
    public List<DecorateTile> GetObjectsOnTiles()
    {
        List<DecorateTile> objectsOnTiles = new List<DecorateTile>();

        //타일맵의 자식 오브젝트를 가져옴
        for (int i = 0; i < tilemap.transform.childCount; i++)
        {
            DecorateTile tile = tilemap.transform.GetChild(i).GetComponent<DecorateTile>();
            
            if (tile.transform.position.y < -2.2f)
            {
                tile.tileType = DecorateType.Water;
            }
            
            objectsOnTiles.Add(tile);
        }
        
        return objectsOnTiles;
    }
}
