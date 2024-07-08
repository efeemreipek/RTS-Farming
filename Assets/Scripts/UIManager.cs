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
    [Header("Node Info Panel")]
    [SerializeField] private GameObject nodeInfoPanel;
    [SerializeField] private TextMeshProUGUI nodeNameText;
    // IF NODE IS RESOURCE NODE
    [SerializeField] private TextMeshProUGUI currentAmountText;
    [SerializeField] private TextMeshProUGUI maxAmountText;
    //IF NODE IS STORAGE NODE
    [SerializeField] private TextMeshProUGUI resourceAmountText;


    public TextMeshProUGUI GetGoldAmountText() => goldAmountText;
    public void UpdateGoldAmountText(string text)
    {
        goldAmountText.text = text;
    }
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
            currentAmountText.text = "Current: " + resourceNode.GetCurrentGoldAmount().ToString();
            maxAmountText.text = "Max: " + resourceNode.GetMaxGoldAmount().ToString();
            resourceAmountText.text = "";
        }
        else if(node is StorageNode)
        {
            StorageNode storageNode = (StorageNode)node;
            currentAmountText.text = "";
            maxAmountText.text = "";
            resourceAmountText.text = "Gold: " + GameResources.goldAmount;
        }
        nodeInfoPanel.SetActive(true);
    }
}
