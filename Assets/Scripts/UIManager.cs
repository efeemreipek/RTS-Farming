using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    [Header("Resources")]
    [SerializeField] private TextMeshProUGUI stoneAmountText;
    [SerializeField] private TextMeshProUGUI woodAmountText;
    [SerializeField] private TextMeshProUGUI foodAmountText;
    [Header("Node Info Panel")]
    [SerializeField] private GameObject nodeInfoPanel;
    [SerializeField] private TextMeshProUGUI nodeNameText;
    // IF NODE IS RESOURCE NODE
    [SerializeField] private TextMeshProUGUI currentAmountText;
    [SerializeField] private TextMeshProUGUI maxAmountText;
    // IF NODE IS MAIN BUILDING
    [SerializeField] private GameObject spawnUnitButtonGO;
    [SerializeField] private Image unitSpawnTimerImage;
    [Header("Building")]
    [SerializeField] private GameObject buildingSettingsPanel;



    public TextMeshProUGUI GetStoneAmountText() => stoneAmountText;
    public TextMeshProUGUI GetWoodAmountText() => woodAmountText;
    public TextMeshProUGUI GetFoodAmountText() => foodAmountText;

    public GameObject GetNodeInfoPanel() => nodeInfoPanel;
    public void SetActiveNodeInfoPanel(bool cond) => nodeInfoPanel.SetActive(cond);
    public TextMeshProUGUI GetNodeNameText() => nodeNameText;

    public TextMeshProUGUI GetCurrentAmountText() => currentAmountText;
    public TextMeshProUGUI GetMaxAmountText() => maxAmountText;

    public GameObject GetSpawnUnitButtonGO() => spawnUnitButtonGO;
    public Image GetUnitSpawnTimerImage() => unitSpawnTimerImage;

    public GameObject GetBuildingSettingsPanel() => buildingSettingsPanel;

    public void InitializeNodeInfoPanel(Node node)
    {
        nodeNameText.text = node.GetNodeName().ToUpperInvariant();
        if(node is ResourceNode)
        {
            ResourceNode resourceNode = (ResourceNode)node;
            currentAmountText.text = "CURRENT: " + resourceNode.GetCurrentResourceAmount().ToString();
            maxAmountText.text = "MAX: " + resourceNode.GetMaxResourceAmount().ToString();
            spawnUnitButtonGO.SetActive(false);
        }
        else if(node is FoodHouseNode)
        {
            FoodHouseNode foodHouseNode = (FoodHouseNode)node;
            currentAmountText.text = "";
            maxAmountText.text = "";
            spawnUnitButtonGO.SetActive(false);
        }
        else if (node is MainBuildingNode)
        {
            MainBuildingNode mainBuildingNode = (MainBuildingNode)node;
            currentAmountText.text = "";
            maxAmountText.text = "";
            spawnUnitButtonGO.SetActive(true);
        }
        SetActiveNodeInfoPanel(true);
    }
}
