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

    private List<ResourceNode> allNodesList = new List<ResourceNode>();
    private List<ResourceNode> selectedNodesList = new List<ResourceNode>();

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
    }

    private void OnDisable()
    {
        mouseLeftClick.performed -= MouseLeftClick_Performed;
        gameControlActions.Disable();
    }

    private void Start()
    {
        GameObject[] allNodesGO = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject nodeGO in allNodesGO)
        {
            allNodesList.Add(nodeGO.GetComponent<ResourceNode>());
        }
    }

    private Vector2 GetMousePosition() => mousePosition.ReadValue<Vector2>();

    private void MouseLeftClick_Performed(InputAction.CallbackContext inputValue)
    {
        Ray ray = _camera.ScreenPointToRay(GetMousePosition());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.TryGetComponent(out ResourceNode resourceNode)) // Clicked on a resource node
            {
                if (Keyboard.current.leftShiftKey.isPressed) // Multiple resource node selection with shift
                {
                    SelectMultipleUnitWithShift(resourceNode);
                }
                else // Single resource node selection
                {
                    SelectSingleUnit(resourceNode);
                }

                Debug.Log("You clicked on " + hit.collider.gameObject.name);
            }
            else // Clicked on something else. So deselect all resource nodes
            {
                DeselectAllNodes();
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

    private void SelectSingleUnit(ResourceNode node)
    {
        DeselectAllNodes();
        selectedNodesList.Add(node);

        foreach (ResourceNode _node in allNodesList)
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

    private void SelectMultipleUnitWithShift(ResourceNode node)
    {
        if (!selectedNodesList.Contains(node))
        {
            selectedNodesList.Add(node);
        }

        foreach (ResourceNode _node in allNodesList)
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
}
