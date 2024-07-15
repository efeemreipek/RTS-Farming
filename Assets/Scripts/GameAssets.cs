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

    public Material goldMaterial;
    public Material goldEmptyMaterial;
    public ResourceData goldResourceData;
    public ResourceData woodResourceData;
    public ResourceData foodResourceData;
}
