using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, ISelectable
{
    public static Action OnResourceGathered;

    public enum State
    {
        Idle,
        Moving,
        MovingToResource,
        MovingToFoodHouse,
        GatheringResource
    }

    [SerializeField] private GameObject selectedQuad;

    private UnitAnimator _unitAnimator;
    private NavMeshAgent _unitNMA;
    private ResourceNode _resourceNode;
    private FoodHouseNode _foodHouseNode;
    private State _currentState;
    private Vector3 _targetPosition;

    private bool _isThisUnitSelected = false;
    private bool _isGatheringResource = false;
    private bool _canMove = true;
    //private int _currentResourceAmount = 0;
    //private int _maxResourceAmount = 3;
    private int _currentEnergy = 0;
    private int _maxEnergy = 5;

    private void Awake()
    {
        _unitAnimator = GetComponent<UnitAnimator>();
        _unitNMA = GetComponent<NavMeshAgent>();

        SetEnergyToMax();

        _currentState = State.Idle;
    }

    private void Update()
    {
        _unitAnimator.SetIsWalking(IsNMAMoving());

        switch (_currentState)
        {
            case State.Idle:
                break;
            case State.Moving:
                if (CheckIfDestinationIsReached())
                {
                    ChangeState(State.Idle);
                }
                break;
            case State.MovingToResource:
                if (CheckIfDestinationIsReached())
                {
                    RotateTowardsResource();
                    ChangeState(State.GatheringResource);
                }
                break;
            case State.MovingToFoodHouse:
                if (CheckIfDestinationIsReached())
                {
                    StartCoroutine(RestoreEnergy());
                }
                break;
            case State.GatheringResource:
                RotateTowardsResource();
                StartCoroutine(GatherResource(_resourceNode));
                break;
        }
    }

    public bool IsSelected(bool cond)
    {
        _isThisUnitSelected = cond;
        selectedQuad.SetActive(_isThisUnitSelected);
        return _isThisUnitSelected;
    }
    private bool CheckIfDestinationIsReached()
    {
        if (!_unitNMA.pathPending)
        {
            if (_unitNMA.remainingDistance <= _unitNMA.stoppingDistance)
            {
                if (!_unitNMA.hasPath || _unitNMA.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void ChangeState(State newState)
    {
        if (_currentState == newState) return;
        _currentState = newState;
    }
    private bool IsNMAMoving()
    {
        return _unitNMA.velocity.sqrMagnitude > 0f;
    }
    private void SetEnergyToMax()
    {
        _currentEnergy = _maxEnergy;
    }

    public void MoveTo(Vector3 position)
    {
        if (!_canMove) return;

        _targetPosition = position;
        _unitNMA.SetDestination(_targetPosition);
        ChangeState(State.Moving);

        _resourceNode = null;
    }
    public void MoveToGatherResource(ResourceNode resourceNode)
    {
        _resourceNode = resourceNode;
        _targetPosition = FindClosestNodeMovePoint(_resourceNode);

        _unitNMA.SetDestination(_targetPosition);
        ChangeState(State.MovingToResource);
    }
    private void MoveToFoodHouse()
    {
        _foodHouseNode = FindClosesetFoodHouseNode();
        _targetPosition = FindClosestNodeMovePoint(_foodHouseNode);

        _unitNMA.SetDestination(_targetPosition);
        ChangeState(State.MovingToFoodHouse);
    }
    private IEnumerator GatherResource(ResourceNode resourceNode)
    {
        if (!_isGatheringResource && /*_currentResourceAmount < _maxResourceAmount &&*/ _currentEnergy > 0 && resourceNode.CanGatherResource())
        {
            _canMove = false;
            if(resourceNode is GoldResourceNode)
            {
                _unitAnimator.TriggerMine();
            }
            else if(resourceNode is WoodResourceNode)
            {
                _unitAnimator.TriggerCut();
            }
            else if(resourceNode is FishResourceNode)
            {
                _unitAnimator.TriggerFish();
            }
            _isGatheringResource = true;
            yield return new WaitForSeconds(_unitAnimator.GetCurrentAnimationLength());
            _canMove = true;
        }
        if (!resourceNode.CanGatherResource())
        {
            MoveToFoodHouse();
        }
    }
    private FoodHouseNode FindClosesetFoodHouseNode()
    {
        FoodHouseNode closestFoodHouseNode = null;
        GameObject[] allNodes = GameObject.FindGameObjectsWithTag("Node");
        List<FoodHouseNode> foodHouseNodes = new List<FoodHouseNode>();
        foreach (GameObject node in allNodes)
        {
            if (node.TryGetComponent(out FoodHouseNode _foodHouseNode))
            {
                foodHouseNodes.Add(_foodHouseNode);
            }
        }

        float closestDistance = float.MaxValue;
        foreach (FoodHouseNode foodHouseNode in foodHouseNodes)
        {
            Vector3 direction = foodHouseNode.transform.position - transform.position;
            float distance = direction.sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestFoodHouseNode = foodHouseNode;
            }
        }

        return closestFoodHouseNode;
    }
    private Vector3 FindClosestNodeMovePoint(Node node)
    {
        Vector3 closestMovePoint = Vector3.zero;
        float closestDistance = float.MaxValue;
        foreach (Vector3 movePoint in node.GetMovePointList())
        {
            Vector3 direction = movePoint - transform.position;
            float distance = direction.sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestMovePoint = movePoint;
            }
        }

        return closestMovePoint;
    }
    private void RotateTowardsResource()
    {
        if (_resourceNode == null) return;

        Vector3 direction = (_resourceNode.transform.position - transform.position).normalized;
        direction.y = 0; // Keep the rotation on the horizontal plane

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _unitNMA.angularSpeed);
    }
    private IEnumerator RestoreEnergy()
    {
        ChangeState(State.Idle);

        while(_currentEnergy < _maxEnergy)
        {
            yield return new WaitForSeconds(1f);
            _currentEnergy++;
            print("++");
        }

        yield return null;

        if (_resourceNode.CanGatherResource())
        {
            MoveToGatherResource(_resourceNode);
        }
        else
        {
            ChangeState(State.Idle);
        }
    }

    public void OnAnimationEnd()
    {
        _isGatheringResource = false;
        _resourceNode.DecrementResourceAmount();
        OnResourceGathered?.Invoke();
        _currentEnergy = Mathf.Max(_currentEnergy - 1, 0);
        if(_currentEnergy <= 0)
        {
            MoveToFoodHouse();
        }
        else
        {
            ChangeState(State.GatheringResource);
        }
    }
}
