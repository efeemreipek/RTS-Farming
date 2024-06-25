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
            if (hit.collider.CompareTag("Ground"))
            {
                Unit selectedUnit = unitSelector.GetSelectedUnit();
                selectedUnit.GetComponent<NavMeshAgent>().SetDestination(hit.point);
            }
        }
    }

    private Vector2 GetMousePosition()
    {
        return mousePosition.ReadValue<Vector2>();
    }
}
