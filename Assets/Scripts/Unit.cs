using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, ISelectable
{
    private enum State
    {
        Idle,
        Moving,
        MovingToResource,
        Gathering
    }

    [SerializeField] private GameObject selectedQuad;

    private UnitAnimator unitAnimator;
    private NavMeshAgent unitNMA;

    private bool isThisUnitSelected = false;
    private Vector3 targetPosition;
    private State currentState;
    private int goldAmount = 0;
    private bool isMining = false;

    private void Awake()
    {
        unitAnimator = GetComponent<UnitAnimator>();
        unitNMA = GetComponent<NavMeshAgent>();

        currentState = State.Idle;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                unitAnimator.SetIsWalking(false);
                break;
            case State.Moving:
                unitAnimator.SetIsWalking(true);
                if (CheckIfDestinationIsReached())
                {
                    currentState = State.Idle;
                }
                break;
            case State.MovingToResource:
                unitAnimator.SetIsWalking(true);
                if (CheckIfDestinationIsReached())
                {
                    currentState = State.Gathering;
                }
                break;
            case State.Gathering:
                GatherResource();
                break;
            default:
                break;
        }

    }
    
    public bool IsSelected(bool cond)
    {
        isThisUnitSelected = cond;
        selectedQuad.SetActive(isThisUnitSelected);
        return isThisUnitSelected;
    }

    private bool IsIdle() => unitNMA.velocity.sqrMagnitude <= 0.01f;

    public void MoveTo(Vector3 position)
    {
        currentState = State.Moving;
        targetPosition = position;
        unitNMA.SetDestination(targetPosition);
    }

    public void MoveToGatherResource(Vector3 position)
    {
        currentState = State.MovingToResource;
        targetPosition = position;
        unitNMA.SetDestination(targetPosition);
    }

    private bool CheckIfDestinationIsReached()
    {
        // Check if we've reached the destination
        if (!unitNMA.pathPending)
        {
            if (unitNMA.remainingDistance <= unitNMA.stoppingDistance)
            {
                if (!unitNMA.hasPath || unitNMA.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void GatherResource()
    {
        if(!isMining)
        {
            unitAnimator.TriggerMine();
            isMining = true;
        }
        else
        {
            currentState = State.Idle;
        }
    }

    public void OnMineAnimationEnd()
    {
        goldAmount++;
        isMining = false;
    }
}
