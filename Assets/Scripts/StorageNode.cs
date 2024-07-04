using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageNode : Node
{
    private int goldAmount;

    public void AddGoldToStorage(int amount)
    {
        goldAmount += amount;
    }
}
