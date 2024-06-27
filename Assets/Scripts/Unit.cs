using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public enum State
    {
        Idle,
        MovingToSomewhere,
        MovingToResource,
        MovingToStorage,
        GatheringResource,
    }

    [SerializeField] private GameObject selectedQuad;

    private UnitAnimator unitAnimator;
    private NavMeshAgent unitNMA;
    private State state;

    private bool isThisUnitSelected = false;

    private void Awake()
    {
        unitAnimator = GetComponent<UnitAnimator>();
        unitNMA = GetComponent<NavMeshAgent>();

        state = State.Idle;
    }

    private void Update()
    {
        selectedQuad.SetActive(isThisUnitSelected);

        if (IsIdle())
        {
            // Unit is not moving, so it is idle
            unitAnimator.SetIsWalking(false);
            ChangeState(State.Idle);
        }
        else
        {
            // Unit is moving
            unitAnimator.SetIsWalking(true);
        }

        switch (state)
        {
            case State.Idle:
                Debug.Log("Idle State");
                break;
            case State.MovingToSomewhere:
                Debug.Log("Moving To Somewhere State");
                break;
            case State.MovingToResource:
                Debug.Log("Moving To Resource State");
                break;
        }
    }


    public void SetThisUnitSelected(bool cond) => isThisUnitSelected = cond;
    public void MoveTo(Vector3 position)
    {
        unitNMA.SetDestination(position);
    }
    public bool IsIdle() => unitNMA.velocity.magnitude <= 0f;
    public void ChangeState(State newState)
    {
        if (state != newState)
        {
            state = newState;
        }
    }
}
