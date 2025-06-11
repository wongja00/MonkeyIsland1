using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecoCard : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI Desc;
    public Image Thumbnail;
    public Button decoButton;
    
    void Init(DecoData decoData)
    {
        NameText.text = decoData.Deco_Name;
        Thumbnail.sprite = (DataContainer.Instance[DataContainerSetID.Deco][decoData.ID] as DecoContaner).Thumbnail;
    }
    
    public void SetDecoration()
    {
        TileMap.instance.DecorateMode(true);
    }
    
    public void ActiveAbility(DecoData decoData)
    {
        //DecorateManager.instance.Dec;
    }
    
}
