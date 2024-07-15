using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : Node
{
    public static event Action<ResourceData> OnResourceGathered;
    public ResourceData ResourceData;

    private int _maxResourceAmount = 6;
    private int _currentResourceAmount;
    private int _regenerationTime = 15;

    protected override void Start()
    {
        base.Start();

        SetCurrentToMax();
    }

    public void DecrementResourceAmount()
    {
        _currentResourceAmount = Mathf.Max(_currentResourceAmount - 1, 0);

        OnResourceGathered?.Invoke(ResourceData);

        if(_currentResourceAmount <= 0)
        {
            StartCoroutine(RegenerateResource());
        }
    }
    public bool CanGatherResource() => _currentResourceAmount > 0;
    public int GetMaxResourceAmount() => _maxResourceAmount;
    public int GetCurrentResourceAmount() => _currentResourceAmount;
    public void SetCurrentToMax() => _currentResourceAmount = _maxResourceAmount;
    private IEnumerator RegenerateResource()
    {
        yield return new WaitForSeconds(_regenerationTime);
        SetCurrentToMax();
    }

}
