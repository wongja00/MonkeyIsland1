using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//자식오브젝트의 유닛카드를 인덱스에 따라 정렬
public class SortByIndex : MonoBehaviour
{
    public Transform parent;
    public List<Transform> childList = new List<Transform>();
    public List<int> indexList = new List<int>();

    public int mType = 0;
    
    private void Start()
    {
        indexList = CSVReader.GetMonkeyHarvestOrder(mType);

        if (mType != 1)
        {
            for (int i = 0; i < indexList.Count; i++)
            {
                indexList[i] -= 15;
            }
        }
        
        SetSort();
    }
    
    public void SetSort()
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            childList.Add(parent.GetChild(i));
        }

        // customOrder 리스트에 따라 자식 오브젝트 정렬
        for (int i = 0; i < childList.Count; i++)
        {
            if(indexList[i] < childList.Count)
                childList[indexList[i] - 1].SetSiblingIndex(i);
        }
    }
    
}
