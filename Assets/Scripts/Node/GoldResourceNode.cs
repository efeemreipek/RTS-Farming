using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldResourceNode : ResourceNode
{

    private void Update()
    {
        if (!CanGatherResource())
        {
            ChangeMaterialOfVisual();
        }
    }

    private void ChangeMaterialOfVisual()
    {
        Transform visualTransform = transform.GetChild(1);
        List<MeshRenderer> visualMeshRendererList = new List<MeshRenderer>();
        for (int i = 0; i < visualTransform.childCount; i++)
        {
            visualMeshRendererList.Add(visualTransform.GetChild(i).GetComponent<MeshRenderer>());
        }

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
