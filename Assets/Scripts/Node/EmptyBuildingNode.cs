using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBuildingNode : Node
{
    public static Action<Node> OnNodeDestroyed;

    private Node nodeToBuild = null;

    public void SetBuildNode(Node node)
    {
        nodeToBuild = node;

        string newName = GetNodeName() + $" ({node.GetNodeName()})";
        SetNodeName(newName);
    }
    public void Build()
    {
        Instantiate(nodeToBuild, transform.position, Quaternion.identity);
        OnNodeDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
    public Node GetNodeToBuild() => nodeToBuild;
}
