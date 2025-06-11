using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Harvester클래스를 지닌 프리팹을 담는 컨테이너
[CreateAssetMenu(fileName = "HarvestUnitContainer", menuName = "UnitContainer/HarvestUnitContainer")]
public class HarvestContainer : ScriptableObject
{
    public List<Harvesterprefab> harvesters = new List<Harvesterprefab>();

    public Harvesterprefab this[int ID]
    {
        get { return harvesters.Find(x => x.ID == ID); }
    }
    
}
//Harvester클래스
[System.Serializable]
public class Harvesterprefab
{
    public int ID;
    public GameObject prefab;
}
