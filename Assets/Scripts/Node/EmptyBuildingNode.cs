using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBuildingNode : Node
{
    private Node nodeToBuild = null;

    public void SetBuildNode(Node node)
    {
        nodeToBuild = node;

        string newName = GetNodeName() + $" ({node.GetNodeName()})";
        SetNodeName(newName);
    }
}
