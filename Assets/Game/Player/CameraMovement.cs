using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class CameraMovement : MonoBehaviour
{
    [Inject]
    private PlayerControls controls;

    [SerializeField]
    private float cameraSpeed = 0.1f;

    [SerializeField]
    private float controllerSpeed = 20.0f;

    private Vector2 _move;

    // Start is called before the first frame update
    void Awake()
    {
        controls.Camera.LookMouse.performed += ctx => OnMouseCamera();
        controls.Camera.Look.performed += ctx => OnMoveCamera();
        _move = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (_move != Vector2.zero)
        {
            _move = controls.Camera.Look.ReadValue<Vector2>();
            gameObject.transform.Rotate(new Vector3(0, (_move.x * -1.0f) * controllerSpeed * cameraSpeed * Time.deltaTime, 0));
        }
    }

    private void OnMouseCamera()
    {
        Vector2 move = controls.Camera.LookMouse.ReadValue<Vector2>();
        gameObject.transform.Rotate(new Vector3(0, move.x * cameraSpeed * Time.deltaTime, 0));
    }

    private void OnMoveCamera()
    {
        _move = controls.Camera.Look.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        controls.Camera.Enable();
    }

    private void OnDisable()
    {
        controls.Camera.Disable();
    }
}
