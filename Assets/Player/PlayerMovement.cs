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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _body.AddForce(_move.x * _speed, 0, _move.y * _speed);
    }

    private void OnMove(InputValue playerMovement)
    {
        Vector2 movement = playerMovement.Get<Vector2>();
        _move.x = movement.x;
        _move.y = movement.y;
        Debug.Log(movement);
    }
}
