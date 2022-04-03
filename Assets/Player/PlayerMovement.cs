using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _body;

    [SerializeField]
    private GameObject _cameraLocker;

    // Standard player movement when not climbing, balancing, etc.
    private Vector2 _move;

    // Forward movement for climbing, balancing, and shimmying
    private float forward;

    [SerializeField]
    private float _speed = 10.0f;

    [SerializeField]
    private float _jumpForce = 10.0f;

    [SerializeField]
    private float _playerHeight = 2.0f;

    private PlayerControls controls;

    [Inject]
    private SignalBus sb;

    // // Variables for dealing with moving up ladders // //
    private LadderInteractionSignal lis;

    [SerializeField]
    private float _climbSpeed = 0.5f;

    private bool _ladderMode;

    private bool _ladderStart;

    private bool _ladderEnd;

    private Vector3 _lerpStart;

    private float _lerpTimer;

    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Jump.performed += ctx => OnJump();
        controls.Player.Move.performed += ctx => OnMove();
        controls.Player.Climb.performed += ctx => OnClimb();
        _ladderStart = false;
    }

    void Start()
    {
        sb.Subscribe<LadderInteractionSignal>((lis) => { this.lis = lis; _ladderStart = true; });
    }

    void Update()
    {
        gameObject.transform.Translate(Vector3.zero);

        // Ladder stuff
        if (_ladderStart)
        {
            gameObject.transform.position = lis.ladderRailBottom;
            controls.Player.Jump.Disable();
            controls.Player.Move.Disable();
            controls.Player.Climb.Enable();
            _ladderStart = false;
            _ladderEnd = false;
            _ladderMode = true;
            _body.isKinematic = true;
            forward = 0;
        } 
        else if (_ladderMode)
        {
            float movement = forward;
            Debug.Log(Vector3.up * (movement * _climbSpeed * Time.deltaTime));
            gameObject.transform.Translate(Vector3.up * (movement * _climbSpeed * Time.deltaTime));
            if (gameObject.transform.position.y < lis.ladderRailBottom.y)
            {
                Debug.Log("Bottom of the ladder!");
                gameObject.transform.position = lis.ladderRailBottom;
            } else if (gameObject.transform.position.y >= lis.ladderRailTop.y)
            {
                Debug.Log("Done with ladder!");
                _ladderMode = false;
                _ladderEnd = true;
                _lerpStart = gameObject.transform.position;
                _lerpTimer = 0;
            }
        }
        else if (_ladderEnd)
        {
            _lerpTimer += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(_lerpStart, lis.ladderFinish, _lerpTimer);
            if (_lerpTimer >= 1)
            {
                _ladderEnd = false;
                controls.Player.Jump.Enable();
                controls.Player.Move.Enable();
                controls.Player.Climb.Disable();
                _body.isKinematic = false;
            }
        }
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

    private void OnClimb()
    {
        float movement = controls.Player.Climb.ReadValue<float>();
        forward = movement;
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
