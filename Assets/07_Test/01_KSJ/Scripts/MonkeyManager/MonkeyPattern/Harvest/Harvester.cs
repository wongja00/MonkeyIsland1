using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvester :  MonkeyParent
{
    
    private void Start()
    {
        AdjustPivotToBottomCenter();
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, 0);
    }

    public void Harvest()
    {
        state = MonkeyState.Moving;
        targetTree.HarvestBananas(0);
    }
    
    
}
