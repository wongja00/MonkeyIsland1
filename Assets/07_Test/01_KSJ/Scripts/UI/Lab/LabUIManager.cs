using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LabUIManager : MonoBehaviour
{
    public static LabUIManager instance;

    //ȯ�� ����Ʈ
    [SerializeField]
    private TextMeshProUGUI rebirthPointText;
    public int rebirthPoint = 0;

    //��ī�� ������
    [SerializeField] 
    private LabCard prefabLabCard;
    
    //��ī�� �θ�
    [SerializeField]
    private Transform labCardParent;
    
    //�Ҹ� ȯ�� ����Ʈ
    public int consumeRebirthPoint = 0;
    
    //���� �г�
    public GameObject researchPanel;
    
    //����â

    
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
