using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMover : MonoBehaviour
{
    private GameControlActions gameControlActions;
    private InputAction mouseRightClick;
    private InputAction mousePosition;
    private UnitSelector unitSelector;

    private Camera _camera;

    private void Awake()
    {
        gameControlActions = new GameControlActions();
        unitSelector = GetComponent<UnitSelector>();
        _camera = Camera.main;
    }
    

    private void OnEnable()
    {
        mouseRightClick = gameControlActions.Game.MouseRightClick;
        mousePosition = gameControlActions.Game.MousePosition;

        mouseRightClick.performed += MouseRightClick_Performed;
        gameControlActions.Enable();
    }

    private void OnDisable()
    {
        mouseRightClick.performed -= MouseRightClick_Performed;
        gameControlActions.Disable();
    }

    private void MouseRightClick_Performed(InputAction.CallbackContext inputValue)
    {
        Ray ray = _camera.ScreenPointToRay(GetMousePosition());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Right clicked resource node to gather resource
            if (hit.collider.TryGetComponent(out ResourceNode resourceNode))
            {
                List<Unit> selectedUnitsList = unitSelector.GetSelectedUnitsList();
                foreach (Unit selectedUnit in selectedUnitsList)
                {
                    if (selectedUnit != null)
                    {
                        selectedUnit.MoveToGatherResource(resourceNode.transform.position);
                    }
                }
            }
            // Right clicked ground to move
            else if (hit.collider.CompareTag("Ground"))
            {
                List<Unit> selectedUnitsList = unitSelector.GetSelectedUnitsList();
                foreach(Unit selectedUnit in selectedUnitsList)
                {
                    if (selectedUnit != null)
                    {
                        selectedUnit.MoveTo(hit.point);
                    }
                }
            }
            // Right clicked storage node empty inventory
            else if (hit.collider.TryGetComponent(out StorageNode storageNode))
            {
                List<Unit> selectedUnitsList = unitSelector.GetSelectedUnitsList();
                foreach (Unit selectedUnit in selectedUnitsList)
                {
                    if (selectedUnit != null)
                    {
                        selectedUnit.MoveToStorage(storageNode.transform.position);
                    }
                }
            }
        }
    }

    private Vector2 GetMousePosition()
    {
        return mousePosition.ReadValue<Vector2>();
    }
}
