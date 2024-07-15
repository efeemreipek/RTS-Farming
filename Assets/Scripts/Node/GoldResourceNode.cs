using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldResourceNode : ResourceNode
{
    private Transform visualTransform;
    private List<MeshRenderer> visualMeshRendererList;

    protected override void Start()
    {
        base.Start();

        visualTransform = transform.GetChild(1);
        visualMeshRendererList = new List<MeshRenderer>();
        for (int i = 0; i < visualTransform.childCount; i++)
        {
            visualMeshRendererList.Add(visualTransform.GetChild(i).GetComponent<MeshRenderer>());
        }
    }

    private void Update()
    {
        ChangeMaterialOfVisual();
    }

    private void ChangeMaterialOfVisual()
    {
        if (!CanGatherResource())
        {
            foreach (MeshRenderer renderer in visualMeshRendererList)
            {
                renderer.material = GameAssets.Instance.goldEmptyMaterial;
            }
        }
        else
        {
            foreach (MeshRenderer renderer in visualMeshRendererList)
            {
                renderer.material = GameAssets.Instance.goldMaterial;
            }
        }
    }
}
