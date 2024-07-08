using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : Node
{
    private int _maxGoldAmount = 5;
    private int _currentGoldAmount;
    private int _regenerationTime = 15;

    protected override void Start()
    {
        base.Start();

        SetCurrentToMax();
    }

    public void DecrementGoldAmount()
    {
        _currentGoldAmount = Mathf.Max(_currentGoldAmount - 1, 0);
        if(_currentGoldAmount <= 0)
        {
            StartCoroutine(RegenerateResource());
            ChangeMaterialOfVisual();
        }
    }
    public bool CanGatherResource() => _currentGoldAmount > 0;
    public int GetMaxGoldAmount() => _maxGoldAmount;
    public int GetCurrentGoldAmount() => _currentGoldAmount;
    public void SetCurrentToMax() => _currentGoldAmount = _maxGoldAmount;
    private IEnumerator RegenerateResource()
    {
        yield return new WaitForSeconds(_regenerationTime);
        SetCurrentToMax();
        ChangeMaterialOfVisual();
    }
    private void ChangeMaterialOfVisual()
    {
        Transform visualTransform = transform.GetChild(1);
        List<MeshRenderer> visualMeshRendererList = new List<MeshRenderer>();
        for(int i = 0; i < visualTransform.childCount; i++)
        {
            visualMeshRendererList.Add(visualTransform.GetChild(i).GetComponent<MeshRenderer>());
        }

        if (!CanGatherResource())
        {
            foreach(MeshRenderer renderer in visualMeshRendererList)
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
