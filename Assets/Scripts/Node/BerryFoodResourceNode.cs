using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryFoodResourceNode : FoodResourceNode
{
    private void Update()
    {
        if (!CanGatherResource())
        {
            ChangeVisual(true);
        }
        else
        {
            ChangeVisual(false);
        }
    }

    private void ChangeVisual(bool cond)
    {
        Transform visualTransform = transform.GetChild(1);
        Transform noResourceVisualTransform = visualTransform.GetChild(0);
        Transform ResourceVisualTransform = visualTransform.GetChild(1);

        noResourceVisualTransform.gameObject.SetActive(cond);
        ResourceVisualTransform.gameObject.SetActive(!cond);
    }
}
