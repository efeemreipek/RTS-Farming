using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystemUI : MonoBehaviour
{
    private BuildingSystem buildingSystem;

    private bool isBuildButtonClicked = false;

    private void Awake()
    {
        buildingSystem = GetComponent<BuildingSystem>();
    }

    public void BuildButtonClicked()
    {
        isBuildButtonClicked = !isBuildButtonClicked;
        UIManager.Instance.GetBuildingSettingsPanel().SetActive(isBuildButtonClicked);
        buildingSystem.enabled = isBuildButtonClicked;
    }

    public void BuildingButtonClicked(int index)
    {
        isBuildButtonClicked = !isBuildButtonClicked;
        UIManager.Instance.GetBuildingSettingsPanel().SetActive(isBuildButtonClicked);
        Debug.Log("clicked to " + buildingSystem.GetBuildingsList()[index].name);
        buildingSystem.ChangeIsBuildingSelected(true);
        buildingSystem.InstantiateBuilding(index);
    }
}
