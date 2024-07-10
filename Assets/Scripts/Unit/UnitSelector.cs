using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelector : MonoBehaviour
{
    [SerializeField] private RectTransform selectionBoxVisual;

    private GameControlActions gameControlActions;
    private InputAction mousePosition;
    private InputAction mouseLeftClick;

    private Camera _camera;
    private List<Unit> allUnitsList = new List<Unit>();
    private List<Unit> selectedUnitsList = new List<Unit>();

    //Multiple selection box variables
    private Rect selectionBox;
    private Vector2 startPosition;
    private Vector2 endPosition;

    private void Awake()
    {
        gameControlActions = new GameControlActions();
        _camera = Camera.main;

        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }

    private void OnEnable()
    {
        mousePosition = gameControlActions.Game.MousePosition;
        mouseLeftClick = gameControlActions.Game.MouseLeftClick;

        mouseLeftClick.started += MouseLeftClick_Started;
        mouseLeftClick.performed += MouseLeftClick_Performed;
        mouseLeftClick.canceled += MouseLeftClick_Canceled;
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
        mouseLeftClick.started -= MouseLeftClick_Started;
        mouseLeftClick.performed -= MouseLeftClick_Performed;
        mouseLeftClick.canceled -= MouseLeftClick_Canceled;
        gameControlActions.Disable();
    }

    private void Update()
    {
        if (mouseLeftClick.IsPressed()) // If mouse left click pressed down then draw selection box visual
        {
            endPosition = GetMousePosition();
            DrawVisual();
            DrawSelection();
        }
    }

    private Vector2 GetMousePosition() => mousePosition.ReadValue<Vector2>();

    private void MouseLeftClick_Started(InputAction.CallbackContext inputValue)
    {
        startPosition = GetMousePosition();
        selectionBox = new Rect();
    }
    private void MouseLeftClick_Canceled(InputAction.CallbackContext inputValue)
    {
        SelectUnitsInSelectionBox();
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
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
            selectedUnitsList[i].IsSelected(false);
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
                _unit.IsSelected(true);
            }
            else
            {
                _unit.IsSelected(false);
            }
        }
    }

    private void SelectMultipleUnitWithShift(Unit unit)
    {
        if (!selectedUnitsList.Contains(unit))
        {
            selectedUnitsList.Add(unit);
        }

        foreach (Unit _unit in allUnitsList)
        {
            if (selectedUnitsList.Contains(_unit))
            {
                _unit.IsSelected(true);
            }
            else
            {
                _unit.IsSelected(false);
            }
        }
    }

    private void SelectMultipleUnitWithBox(Unit unit)
    {
        selectedUnitsList.Add(unit);

        foreach (Unit _unit in allUnitsList)
        {
            if (selectedUnitsList.Contains(_unit))
            {
                _unit.IsSelected(true);
            }
            else
            {
                _unit.IsSelected(false);
            }
        }
    }

    private void DrawVisual()
    {
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        Vector2 boxCenter = (boxStart + boxEnd) * 0.5f;
        selectionBoxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2 (Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        selectionBoxVisual.sizeDelta = boxSize;

    }

    private void DrawSelection()
    {
        if(GetMousePosition().x < startPosition.x) 
        {
            // dragging left
            selectionBox.xMin = GetMousePosition().x;
            selectionBox.xMax = startPosition.x;

        }
        else
        {
            // dragging right
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = GetMousePosition().x;

        }

        if(GetMousePosition().y < startPosition.y)
        {
            // dragging down
            selectionBox.yMin = GetMousePosition().y;
            selectionBox.yMax = startPosition.y;

        }
        else
        {
            // dragging up
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = GetMousePosition().y;

        }
    }

    private void SelectUnitsInSelectionBox()
    {
        foreach(Unit unit in allUnitsList)
        {
            if (selectionBox.Contains(_camera.WorldToScreenPoint(unit.transform.position)))
            {
                SelectMultipleUnitWithBox(unit);
            }
        }
    }
}
