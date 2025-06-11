using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HarvesterŬ������ ���� �������� ��� �����̳�
[CreateAssetMenu(fileName = "HarvestUnitContainer", menuName = "UnitContainer/HarvestUnitContainer")]
public class HarvestContainer : ScriptableObject
{
    public List<Harvesterprefab> harvesters = new List<Harvesterprefab>();

    public Harvesterprefab this[int ID]
    {
        get { return harvesters.Find(x => x.ID == ID); }
    }
    
}
//HarvesterŬ����
[System.Serializable]
public class Harvesterprefab
{
    public int ID;
    public GameObject prefab;
}
