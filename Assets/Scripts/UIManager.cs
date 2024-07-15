using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    [Header("Resources")]
    [SerializeField] private TextMeshProUGUI goldAmountText;
    [SerializeField] private TextMeshProUGUI woodAmountText;
    [SerializeField] private TextMeshProUGUI foodAmountText;
    [Header("Node Info Panel")]
    [SerializeField] private GameObject nodeInfoPanel;
    [SerializeField] private TextMeshProUGUI nodeNameText;
    // IF NODE IS RESOURCE NODE
    [SerializeField] private TextMeshProUGUI currentAmountText;
    [SerializeField] private TextMeshProUGUI maxAmountText;



    public TextMeshProUGUI GetGoldAmountText() => goldAmountText;
    public TextMeshProUGUI GetWoodAmountText() => woodAmountText;
    public TextMeshProUGUI GetFoodAmountText() => foodAmountText;
    public GameObject GetNodeInfoPanel() => nodeInfoPanel;
    public void SetActiveNodeInfoPanel(bool cond) => nodeInfoPanel.SetActive(cond);
    public TextMeshProUGUI GetNodeNameText() => nodeNameText;
    public TextMeshProUGUI GetCurrentAmountText() => currentAmountText;
    public TextMeshProUGUI GetMaxAmountText() => maxAmountText;
    public void InitializeNodeInfoPanel(Node node)
    {
        nodeNameText.text = node.GetNodeName().ToUpper();
        if(node is ResourceNode)
        {
            ResourceNode resourceNode = (ResourceNode)node;
            currentAmountText.text = "CURRENT: " + resourceNode.GetCurrentResourceAmount().ToString();
            maxAmountText.text = "MAX: " + resourceNode.GetMaxResourceAmount().ToString();

        }
        else if(node is FoodHouseNode)
        {
            FoodHouseNode foodHouseNode = (FoodHouseNode)node;
            currentAmountText.text = "";
            maxAmountText.text = "";
        }
        SetActiveNodeInfoPanel(true);
    }
}
