using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private GameControlActions gameControlActions;
    private InputAction moveCamera;
    private Transform cameraTransform;

    //Horizontal motion
    [SerializeField] private float maxSpeed = 5f;
    private float speed;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float damping = 15f;

    //Vertical motion - zooming
    [SerializeField] private float stepSize = 2f;
    [SerializeField] private float zoomDampening = 7.5f;
    [SerializeField] private float minHeight = 5f;
    [SerializeField] private float maxHeight = 50f;
    [SerializeField] private float zoomSpeed = 2f;

    //Rotation
    [SerializeField] private float maxRotationSpeed = 1f;

    //Screen edge motion
    [SerializeField, Range(0f, 0.1f)] private float edgeTolerance = 0.05f;
    [SerializeField] private bool useScreenEdge = true;

    //Value set in various functions.
    //Used to update the position of the camera base object;
    private Vector3 targetPosition;

    private float zoomHeight;

    //Used to track and maintain velocity w/o a rigidbody.
    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;

    //Tracks where the dragging action started.
    private Vector3 startDrag;

    private Transform _transform;


    private void Awake()
    {
        gameControlActions = new GameControlActions();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        _transform = transform;
    }

    private void OnEnable()
    {
        zoomHeight = cameraTransform.localPosition.y;
        cameraTransform.LookAt(_transform);

        lastPosition = _transform.position;

        moveCamera = gameControlActions.Game.MoveCamera;
        gameControlActions.Game.RotateCamera.performed += RotateCamera;
        gameControlActions.Game.ZoomCamera.performed += ZoomCamera;
        gameControlActions.Game.Enable();
    }

    private void OnDisable()
    {
        gameControlActions.Game.RotateCamera.performed -= RotateCamera;
        gameControlActions.Game.ZoomCamera.performed -= ZoomCamera;
        gameControlActions.Game.Disable();
    }


    private void Update()
    {
        GetKeyboardMovement();
        if (useScreenEdge) CheckMouseAtScreenEdge();
    }

    private void LateUpdate()
    {
        UpdateVelocity();
        UpdateCameraPosition();
        UpdateBasePosition();
    }

    private void UpdateVelocity()
    {
        horizontalVelocity = (_transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0f;
        lastPosition = _transform.position;
    }

    private void GetKeyboardMovement()
    {
        Vector2 inputVector2 = moveCamera.ReadValue<Vector2>();
        Vector3 inputValue = inputVector2.x * GetCameraRight() + moveCamera.ReadValue<Vector2>().y * GetCameraForward();
        inputValue = inputValue.normalized;

        if (inputValue.sqrMagnitude > 0.1f)
        {
            targetPosition += inputValue;
        }
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.y = 0f;
        return right;
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        return forward;
    }

    private void UpdateBasePosition()
    {
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            _transform.position += Time.deltaTime * speed * targetPosition;
        }
        else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            _transform.position += horizontalVelocity * Time.deltaTime;
        }

        targetPosition = Vector3.zero;
    }

    private void RotateCamera(InputAction.CallbackContext inputValue)
    {
        if (!Mouse.current.middleButton.isPressed) return;

        float value = inputValue.ReadValue<Vector2>().x;
        _transform.rotation = Quaternion.Euler(0f, value * maxRotationSpeed + _transform.rotation.eulerAngles.y, 0f);
    }

    private void ZoomCamera(InputAction.CallbackContext inputValue)
    {
        float value = -inputValue.ReadValue<Vector2>().y / 100f;

        if (Mathf.Abs(value) > 0.1f)
        {
            zoomHeight = cameraTransform.localPosition.y + value * stepSize;

            if (zoomHeight < minHeight) zoomHeight = minHeight;
            else if (zoomHeight > maxHeight) zoomHeight = maxHeight;
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);
        zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * Vector3.down;

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
        cameraTransform.LookAt(_transform);
    }

    private void CheckMouseAtScreenEdge()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.x < edgeTolerance * Screen.width)
        {
            moveDirection += -GetCameraRight();
        }
        else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
        {
            moveDirection += GetCameraRight();
        }

        if (mousePosition.y < edgeTolerance * Screen.height)
        {
            moveDirection += -GetCameraForward();
        }
        else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
        {
            moveDirection += GetCameraForward();
        }

        targetPosition += moveDirection;
    }
}