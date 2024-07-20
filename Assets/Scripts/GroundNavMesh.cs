using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GroundNavMesh : MonoBehaviour
{
    private NavMeshSurface surface;

    private void OnEnable()
    {
        BuildingSystem.OnNodeBuilt += BuildingSystem_OnNodeBuilt;
    }
    private void OnDisable()
    {
        BuildingSystem.OnNodeBuilt -= BuildingSystem_OnNodeBuilt;
    }
    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
    }


    private void BuildingSystem_OnNodeBuilt(Node obj)
    {
        surface.BuildNavMesh();
    }
}
