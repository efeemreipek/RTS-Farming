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
    private List<Unit> selectedUnitsList = new List<Unit>();

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
            if(hit.collider.TryGetComponent(out Unit unit)) // Clicked on a unit
            {
                if (Keyboard.current.leftShiftKey.isPressed) // Multiple unit selection with shift
                {
                    SelectMultipleUnitWithShift(unit);
                }
                else // Single unit selection
                {
                    SelectSingleUnit(unit);
                }

                Debug.Log("You clicked on " + hit.collider.gameObject.name);
            }
            else // Clicked on something else. So deselect all units
            {
                DeselectAllUnits();
            }
        }

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);
    }

    public List<Unit> GetSelectedUnitsList() => selectedUnitsList;

    private void DeselectAllUnits()
    {
        for (int i = selectedUnitsList.Count - 1; i >= 0; i--)
        {
            selectedUnitsList[i].SetThisUnitUnselected();
            selectedUnitsList.RemoveAt(i);
        }
    }

    private void SelectSingleUnit(Unit unit)
    {
        DeselectAllUnits();
        selectedUnitsList.Add(unit);

        foreach (Unit _unit in allUnitsList)
        {
            if (selectedUnitsList.Contains(_unit))
            {
                _unit.SetThisUnitSelected();
            }
            else
            {
                _unit.SetThisUnitUnselected();
            }
        }
    }

    private void SelectMultipleUnitWithShift(Unit unit)
    {
        selectedUnitsList.Add(unit);

        foreach (Unit _unit in allUnitsList)
        {
            if (selectedUnitsList.Contains(_unit))
            {
                _unit.SetThisUnitSelected();
            }
            else
            {
                _unit.SetThisUnitUnselected();
            }
        }
    }
}
