using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, ISelectable
{
    public enum State
    {
        Idle,
        Moving,
        MovingToResource,
        MovingToStorage,
        GatheringResource
    }

    [SerializeField] private GameObject selectedQuad;

    private UnitAnimator _unitAnimator;
    private NavMeshAgent _unitNMA;
    private StorageNode _storageNode;
    private ResourceNode _resourceNode;
    private State _currentState;
    private Vector3 _targetPosition;

    private bool _isThisUnitSelected = false;
    private bool _isGatheringResource = false;
    private bool _canMove = true;
    private int _currentGoldAmount = 0;
    private int _maxGoldAmount = 3;

    private void Awake()
    {
        _unitAnimator = GetComponent<UnitAnimator>();
        _unitNMA = GetComponent<NavMeshAgent>();

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
                    ChangeState(State.GatheringResource);
                }
                break;
            case State.MovingToStorage:
                if (CheckIfDestinationIsReached())
                {
                    StoreGoldInStorage();

                    if (_resourceNode.CanGatherResource())
                    {
                        MoveToGatherResource(_resourceNode);
                    }
                    else
                    {
                        ChangeState(State.Idle);
                    }
                }
                break;
            case State.GatheringResource:
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

        _targetPosition = resourceNode.transform.position;
        _unitNMA.SetDestination(_targetPosition);
        ChangeState(State.MovingToResource);
    }
    private void MoveToStorage()
    {
        _storageNode = FindClosestStorageNode();

        _targetPosition = _storageNode.transform.position;
        _unitNMA.SetDestination(_targetPosition);
        ChangeState(State.MovingToStorage);
    }
    private IEnumerator GatherResource(ResourceNode resourceNode)
    {
        if (!_isGatheringResource && _currentGoldAmount < _maxGoldAmount && resourceNode.CanGatherResource())
        {
            _canMove = false;
            _unitAnimator.TriggerMine();
            _isGatheringResource = true;
            yield return new WaitForSeconds(_unitAnimator.GetCurrentAnimationLength());
            _canMove = true;
        }
        if (!resourceNode.CanGatherResource())
        {
            MoveToStorage();
        }

    }
    private void StoreGoldInStorage()
    {
        _storageNode.AddGoldToStorage(_currentGoldAmount);
        _currentGoldAmount = 0;
    }
    private bool CheckIfInventoryFull()
    {
        return _currentGoldAmount == _maxGoldAmount;
    }
    private StorageNode FindClosestStorageNode()
    {
        StorageNode closestStorageNode = null;
        GameObject[] allNodes = GameObject.FindGameObjectsWithTag("Node");
        List<StorageNode> storageNodes = new List<StorageNode>();
        foreach (GameObject node in allNodes)
        {
            if (node.TryGetComponent(out StorageNode _storageNode))
            {
                storageNodes.Add(_storageNode);
            }
        }

        float closestDistance = float.MaxValue;
        foreach (StorageNode storageNode in storageNodes)
        {
            Vector3 direction = storageNode.transform.position - transform.position;
            float distance = direction.sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestStorageNode = storageNode;
            }
        }

        return closestStorageNode;
    }

    public void OnMineAnimationEnd()
    {
        _currentGoldAmount++;
        _isGatheringResource = false;
        _resourceNode.DecrementGoldAmount();
        if (CheckIfInventoryFull())
        {
            MoveToStorage();
        }
        else
        {
            ChangeState(State.GatheringResource);
        }
    }
}
