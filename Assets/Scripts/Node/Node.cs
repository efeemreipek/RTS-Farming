using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour, ISelectable
{
    [SerializeField] protected GameObject selectedQuad;
    [SerializeField] protected string nodeName;

    protected bool isThisNodeSelected = false;
    protected List<Vector3> movePointList = new List<Vector3>();

    protected virtual void Start()
    {
        Transform movePointsParent = transform.GetChild(2);
        for (int i = 0; i < movePointsParent.childCount; i++)
        {
            movePointList.Add(movePointsParent.GetChild(i).transform.position);
        }
    }

    public bool IsSelected(bool cond)
    {
        isThisNodeSelected = cond;
        selectedQuad.SetActive(isThisNodeSelected);
        return isThisNodeSelected;
    }
    public List<Vector3> GetMovePointList() => movePointList;
    public string GetNodeName() => nodeName;  
    protected void SetNodeName(string name) => nodeName = name;
}
