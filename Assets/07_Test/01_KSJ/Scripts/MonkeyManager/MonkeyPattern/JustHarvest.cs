using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustHarvest : MonkeyParent // Start is called before the first frame update
{
    private void Start()
    {
        FindBananaTree();
    }

    public void Harvest()
    {
        targetTree.HarvestBananas(0);
    }
}
