using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LabUIManager : MonoBehaviour
{
    public static LabUIManager instance;

    //환생 포인트
    [SerializeField]
    private TextMeshProUGUI rebirthPointText;
    public int rebirthPoint = 0;

    //랩카드 프리팹
    [SerializeField] 
    private LabCard prefabLabCard;
    
    //랩카드 부모
    [SerializeField]
    private Transform labCardParent;
    
    //소모 환생 포인트
    public int consumeRebirthPoint = 0;
    
    //연구 패널
    public GameObject researchPanel;
    
    //연구창

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    public void UpdateRebirthPoint()
    {
        rebirthPointText.text = rebirthPoint.ToString();
    }
    
    public void CreateLabCard()
    {
        LabCard labCard = Instantiate(prefabLabCard, transform);
        labCard.Init();
    }
    
    public void Research(LabCard labCard)
    {
        labCard.level++;
        rebirthPoint -= labCard.cost;
        
        labCard.UpdateUI();
    }
    
    public void UpdateResearchPanel(LabCard labCard)
    {
        
        researchPanel.GetComponentInChildren<TextMeshProUGUI>().text = labCard.description;
    }
}
