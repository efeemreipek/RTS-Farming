using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour, ISelectable
{
    [SerializeField] private GameObject selectedQuad;

    private bool isThisNodeSelected = false;

    public bool IsSelected(bool cond)
    {
        isThisNodeSelected = cond;
        selectedQuad.SetActive(isThisNodeSelected);
        return isThisNodeSelected;
    }
}
