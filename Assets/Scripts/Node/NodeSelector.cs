using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NodeSelector : MonoBehaviour
{
    private GameControlActions gameControlActions;
    private InputAction mousePosition;
    private InputAction mouseLeftClick;

    private Camera _camera;

    private List<Node> allNodesList = new List<Node>();
    private List<Node> selectedNodesList = new List<Node>();

    private bool isPointerOverGameObject = false;

    private void Awake()
    {
        gameControlActions = new GameControlActions();
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        mousePosition = gameControlActions.Game.MousePosition;
        mouseLeftClick = gameControlActions.Game.MouseLeftClick;

        mouseLeftClick.performed += MouseLeftClick_Performed;
        gameControlActions.Enable();

        BuildingSystem.OnNodeBuilt += BuildingSystem_OnNodeBuilt;
    }

    private void OnDisable()
    {
        mouseLeftClick.performed -= MouseLeftClick_Performed;
        gameControlActions.Disable();

        BuildingSystem.OnNodeBuilt -= BuildingSystem_OnNodeBuilt;
    }

    private void Start()
    {
        GameObject[] allNodesGO = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject nodeGO in allNodesGO)
        {
            allNodesList.Add(nodeGO.GetComponent<Node>());
        }
    }

    private void Update()
    {
        isPointerOverGameObject = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    private Vector2 GetMousePosition() => mousePosition.ReadValue<Vector2>();

    private void MouseLeftClick_Performed(InputAction.CallbackContext inputValue)
    {
        Ray ray = _camera.ScreenPointToRay(GetMousePosition());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && !isPointerOverGameObject)
        {
            if (hit.collider.TryGetComponent(out Node node)) // Clicked on a resource node
            {
                if (Keyboard.current.leftShiftKey.isPressed) // Multiple resource node selection with shift
                {
                    SelectMultipleUnitWithShift(node);
                }
                else // Single resource node selection
                {
                    SelectSingleUnit(node);
                    UIManager.Instance.InitializeNodeInfoPanel(node);
                }

                Debug.Log("You clicked on " + hit.collider.gameObject.name);
            }
            else // Clicked on something else. So deselect all resource nodes
            {
                DeselectAllNodes();
                UIManager.Instance.SetActiveNodeInfoPanel(false);
            }
        }

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);
    }

    private void DeselectAllNodes()
    {
        for (int i = selectedNodesList.Count - 1; i >= 0; i--)
        {
            selectedNodesList[i].IsSelected(false);
            selectedNodesList.RemoveAt(i);
        }
    }

    private void SelectSingleUnit(Node node)
    {
        DeselectAllNodes();
        selectedNodesList.Add(node);

        foreach (Node _node in allNodesList)
        {
            if (selectedNodesList.Contains(_node))
            {
                _node.IsSelected(true);
            }
            else
            {
                _node.IsSelected(false);
            }
        }
    }

    private void SelectMultipleUnitWithShift(Node node)
    {
        if (!selectedNodesList.Contains(node))
        {
            selectedNodesList.Add(node);
        }

        foreach (Node _node in allNodesList)
        {
            if (selectedNodesList.Contains(_node))
            {
                _node.IsSelected(true);
            }
            else
            {
                _node.IsSelected(false);
            }
        }
    }

    private void BuildingSystem_OnNodeBuilt(Node obj)
    {
        allNodesList.Add(obj);
    }
}
