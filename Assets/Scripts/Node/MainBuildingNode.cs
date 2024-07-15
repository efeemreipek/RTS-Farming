using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBuildingNode : Node
{
    public static Action<Unit> OnUnitSpawned;

    [SerializeField] private Transform unitPrefab;

    public void SpawnUnit()
    {
        Vector3 pos = GetMovePointList()[0];

        Transform unitTransform = Instantiate(unitPrefab, pos, Quaternion.identity);
        Unit unit = unitTransform.GetComponent<Unit>();
        OnUnitSpawned?.Invoke(unit);
    }
}
