using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesUI : MonoBehaviour
{

    private void Start()
    {
        UpdateResourceUI();
    }
    private void OnEnable()
    {
        Unit.OnResourceGathered += Unit_OnResourceGathered;
        MainBuildingNode.OnUnitSpawned += MainBuildingNode_OnUnitSpawned;
    }
    private void OnDisable()
    {
        Unit.OnResourceGathered -= Unit_OnResourceGathered;
        MainBuildingNode.OnUnitSpawned -= MainBuildingNode_OnUnitSpawned;
    }

    private void Unit_OnResourceGathered()
    {
        UpdateResourceUI();
    }
    private void MainBuildingNode_OnUnitSpawned(Unit unit)
    {
        UpdateResourceUI();
    }

    private void UpdateResourceUI()
    {
        UIManager.Instance.GetStoneAmountText().text = "STONE: " + Inventory.Instance.GetResourceStackSize(GameAssets.Instance.stoneResourceData);
        UIManager.Instance.GetWoodAmountText().text = "WOOD: " + Inventory.Instance.GetResourceStackSize(GameAssets.Instance.woodResourceData);
        UIManager.Instance.GetFoodAmountText().text = "FOOD: " + Inventory.Instance.GetResourceStackSize(GameAssets.Instance.foodResourceData);
    }
}
