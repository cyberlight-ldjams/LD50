using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _body;

    private Vector2 _move;

    [SerializeField]
    private float _speed = 10.0f;

    [SerializeField]
    private float _jumpForce = 10.0f;

    [SerializeField]
    private PlayerControls controls;

    private bool grounded;

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
        if (grounded)
        {
            _body.AddForce(_move.x * _speed, 0, _move.y * _speed);
        }
        if (Mathf.Abs(_body.velocity.y) > 0.1)
        {
            grounded = false;
        } 
        else
        {
            grounded = true;
        }
        Debug.Log(_body.velocity);
    }

    private void OnMove()
    {
        Vector2 movement = controls.Player.Move.ReadValue<Vector2>();
        _move.x = movement.x;
        _move.y = movement.y;
    }

    private void OnJump()
    {
        if (grounded)
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
