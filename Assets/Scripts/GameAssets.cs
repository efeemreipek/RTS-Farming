using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Material stoneMaterial;
    public Material stoneEmptyMaterial;
    public ResourceData stoneResourceData;
    public ResourceData woodResourceData;
    public ResourceData foodResourceData;
    public GameObject emptyBuildingNode;
}
