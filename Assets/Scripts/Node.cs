using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour, ISelectable
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
