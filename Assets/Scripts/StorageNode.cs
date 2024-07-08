using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageNode : Node
{
    public void AddGoldToStorage(int amount)
    {
        GameResources.goldAmount += amount;
        UIManager.Instance.UpdateGoldAmountText(GameResources.goldAmount.ToString());
    }
}
