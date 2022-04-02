using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _body;

    [SerializeField]
    private GameObject _cameraLocker;

    private Vector2 _move;

    [SerializeField]
    private float _speed = 10.0f;

    [SerializeField]
    private float _jumpForce = 10.0f;

    [SerializeField]
    private float _playerHeight = 2.0f;

    private PlayerControls controls;

    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Jump.performed += ctx => OnJump();
        controls.Player.Move.performed += ctx => OnMove();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsGrounded())
        {
            Vector3 movement = new Vector3(_move.x * _speed, 0, _move.y * _speed);
            movement = Quaternion.Euler(0, _cameraLocker.transform.rotation.eulerAngles.y, 0) * movement;
            _body.AddForce(movement);
        }
    }

    private bool IsGrounded()
    {
        // The player either isn't falling, or is stuck,
        // so they should count as grounded
        if (_body.velocity.y < 0.001f)
        {
            return true;
        }

        Vector3 position = gameObject.transform.position;
        Vector3 direction = Vector3.down;
        float distance = _playerHeight / 2.0f;

        if (Physics.Raycast(position, direction, distance)) {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnMove()
    {
        Vector2 movement = controls.Player.Move.ReadValue<Vector2>();
        _move.x = movement.x;
        _move.y = movement.y;
    }

    private void OnJump()
    {
        if (IsGrounded())
        {
            _body.AddForce(0, _jumpForce, 0);
        }
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
