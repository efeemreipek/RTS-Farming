using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ResourceData ResourceData;
    public int StackSize;

    public InventoryItem(ResourceData resourceData)
    {
        ResourceData = resourceData;
        AddToStack();
    }

    public void AddToStack()
    {
        StackSize++;
    }
    public void RemoveFromStack()
    {
        StackSize--;
    }
}
