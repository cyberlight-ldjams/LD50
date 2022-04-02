using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    private PlayerControls controls;

    [SerializeField]
    private float cameraSpeed = 0.1f;

    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Look.performed += ctx => OnMoveCamera();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMoveCamera()
    {
        Vector2 move = controls.Player.Look.ReadValue<Vector2>();
        Debug.Log(move);
        gameObject.transform.Rotate(new Vector3(0, move.x * cameraSpeed, 0));
    }


    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
