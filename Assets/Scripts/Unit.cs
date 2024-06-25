using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private GameObject selectedQuad;

    private bool isThisUnitSelected = false;

    private void Update()
    {
        selectedQuad.SetActive(isThisUnitSelected);
    }


    public void SetThisUnitSelected() => isThisUnitSelected = true;
    public void SetThisUnitUnselected() => isThisUnitSelected = false;
}
