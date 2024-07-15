using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesUI : MonoBehaviour
{

    private void Start()
    {
        UIManager.Instance.GetGoldAmountText().text = "GOLD: 0";
        UIManager.Instance.GetWoodAmountText().text = "WOOD: 0";
        UIManager.Instance.GetFoodAmountText().text = "FOOD: 0";
    }
    private void OnEnable()
    {
        Unit.OnResourceGathered += Unit_OnResourceGathered;
    }
    private void OnDisable()
    {
        Unit.OnResourceGathered -= Unit_OnResourceGathered;
    }

    private void Unit_OnResourceGathered()
    {
        UIManager.Instance.GetGoldAmountText().text = "GOLD: " + Inventory.Instance.GetResourceStackSize(GameAssets.Instance.goldResourceData);
        UIManager.Instance.GetWoodAmountText().text = "WOOD: " + Inventory.Instance.GetResourceStackSize(GameAssets.Instance.woodResourceData);
        UIManager.Instance.GetFoodAmountText().text = "FOOD: " + Inventory.Instance.GetResourceStackSize(GameAssets.Instance.foodResourceData);
    }
}
