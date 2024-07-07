using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : Node
{
    private int maxGoldAmount = 5;
    private int currentGoldAmount;

    protected override void Start()
    {
        base.Start();

        currentGoldAmount = maxGoldAmount;
    }

    public void DecrementGoldAmount()
    {
        currentGoldAmount = Mathf.Max(currentGoldAmount - 1, 0);
    }
    public bool CanGatherResource() => currentGoldAmount > 0;
}
