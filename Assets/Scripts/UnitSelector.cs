using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelector : MonoBehaviour
{
    private GameControlActions gameControlActions;
    private InputAction mousePosition;
    private InputAction mouseLeftClick;

    private Camera _camera;
    private List<Unit> allUnitsList = new List<Unit>();
    private Unit selectedUnit;

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

    private void Start()
    {
        GameObject[] allUnitsGO = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject unitGO in allUnitsGO)
        {
            allUnitsList.Add(unitGO.GetComponent<Unit>());
        }
    }

    private void OnDisable()
    {
        mouseLeftClick.performed -= MouseLeftClick_Performed;
        gameControlActions.Disable();
    }

    private Vector2 GetMousePosition()
    {
        return mousePosition.ReadValue<Vector2>();
    }

    private void MouseLeftClick_Performed(InputAction.CallbackContext inputValue)
    {

        Ray ray = _camera.ScreenPointToRay(GetMousePosition());
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(hit.collider.TryGetComponent(out Unit unit))
            {
                selectedUnit = unit;
                foreach(Unit _unit in allUnitsList)
                {
                    if(_unit == selectedUnit)
                    {
                        _unit.SetThisUnitSelected();
                    }
                    else
                    {
                        _unit.SetThisUnitUnselected();
                    }
                }

                Debug.Log("You clicked on " + hit.collider.gameObject.name);
            }
        }

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);
    }

    public Unit GetSelectedUnit() => selectedUnit;
}
