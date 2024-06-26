using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [SerializeField] private GameObject selectedQuad;

    private UnitAnimator unitAnimator;
    private NavMeshAgent unitNMA;

    private bool isThisUnitSelected = false;

    private void Awake()
    {
        unitAnimator = GetComponent<UnitAnimator>();
        unitNMA = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        selectedQuad.SetActive(isThisUnitSelected);
        unitAnimator.SetWalking(unitNMA.velocity.magnitude > 0f);
    }


    public void SetThisUnitSelected() => isThisUnitSelected = true;
    public void SetThisUnitUnselected() => isThisUnitSelected = false;
}
