using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSystem : MonoBehaviour
{
    public static Action<Node> OnNodeBuilt;

    [SerializeField] private List<GameObject> buildingsList = new List<GameObject>();
    [SerializeField] private LayerMask layerMask; 

    private GameControlActions gameControlActions;
    private InputAction mouseLeftClick;
    private InputAction mousePosition;

    private Camera _camera;

    private Vector3 _lastPosition;
    private bool isPointerOverGameObject = false;
    private bool isBuildingSelected = false;
    private GameObject _currentBuilding = null;

    private void Awake()
    {
        gameControlActions = new GameControlActions();
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        mouseLeftClick = gameControlActions.Game.MouseLeftClick;
        mousePosition = gameControlActions.Game.MousePosition;

        mouseLeftClick.performed += MouseLeftClick_Performed;
        gameControlActions.Enable();
    }
    private void OnDisable()
    {
        mouseLeftClick.performed -= MouseLeftClick_Performed;
        gameControlActions.Disable();
    }

    private void MouseLeftClick_Performed(InputAction.CallbackContext obj)
    {
        if (isBuildingSelected)
        {
            Debug.Log("placed on " + GetSelectedMapPosition());

            _currentBuilding.transform.position = GetSelectedMapPosition();
            OnNodeBuilt?.Invoke(_currentBuilding.GetComponent<Node>());
            _currentBuilding = null;

            isBuildingSelected = false;
        }
    }
    private Vector2 GetMousePosition()
    {
        return mousePosition.ReadValue<Vector2>();
    }

    private void Update()
    {
        isPointerOverGameObject = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

        if(_currentBuilding != null)
        {
            _currentBuilding.transform.position = GetSelectedMapPosition();
        }
    }

    private Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePosition = GetMousePosition();
        mousePosition.z = _camera.nearClipPlane;

        Ray ray = _camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) && !isPointerOverGameObject)
        {
            _lastPosition = hit.point;
        }

        return _lastPosition;
    }

    public List<GameObject> GetBuildingsList() => buildingsList;
    public void ChangeIsBuildingSelected(bool cond) => isBuildingSelected = cond;
    public void InstantiateBuilding(int index)
    {
        _currentBuilding = Instantiate(GameAssets.Instance.emptyBuildingNode);
        _currentBuilding.GetComponent<EmptyBuildingNode>().SetBuildNode(buildingsList[index].GetComponent<Node>());
    }
}
