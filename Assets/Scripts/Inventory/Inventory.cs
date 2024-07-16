using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public List<InventoryItem> InventoryItemList = new List<InventoryItem>();

    private Dictionary<ResourceData, InventoryItem> itemDictionary = new Dictionary<ResourceData, InventoryItem>();

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        ResourceNode.OnResourceGathered += Add;
    }
    private void OnDisable()
    {
        ResourceNode.OnResourceGathered -= Add;   
    }

    public void Add(ResourceData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.AddToStack();
            Debug.Log($"{item.ResourceData.DisplayName} total stack is now {item.StackSize}.");
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            InventoryItemList.Add(newItem);
            itemDictionary.Add(itemData, newItem);
            Debug.Log($"Added {itemData.DisplayName} to the inventory for the first time.");
        }
    }

    public void Remove(ResourceData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.RemoveFromStack();
            if (item.StackSize == 0)
            {
                InventoryItemList.Remove(item);
                itemDictionary.Remove(itemData);
            }
        }
    }

    public int GetResourceStackSize(ResourceData resourceData)
    {
        if(itemDictionary.TryGetValue(resourceData, out InventoryItem item))
        {
            return item.StackSize;
        }
        return 0;
    }

    public void RemoveAmount(ResourceData itemData, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Remove(itemData);
        }
    }
}
