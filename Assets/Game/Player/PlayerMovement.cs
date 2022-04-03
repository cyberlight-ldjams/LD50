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

    [SerializeField]
    private float _playerWidth = 1.0f;

    [SerializeField]
    private float _maxVelocity = 10.0f;

    [Inject]
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
        controls.Player.Jump.performed += ctx => OnJump();
        controls.Player.Move.performed += ctx => OnMove();
        controls.Player.Climb.performed += ctx => OnClimb();
        controls.Player.Enable();
        _ladderStart = false;
    }

    void Start()
    {
        sb.Subscribe<LadderInteractionSignal>((lis) => { this.lis = lis; _ladderStart = true; });

        // If we have a spawn point in mind, use it
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        if (spawnPoint != null)
        {
            gameObject.transform.position = spawnPoint.transform.position;
        }
    }

    void Update()
    {
        gameObject.transform.Translate(Vector3.zero);

        // Ladder stuff
        if (_ladderStart)
        {
            Vector3 railStart = lis.ladderRailBottom;
            if (gameObject.transform.position.y > lis.ladderRailBottom.y)
            {
                railStart = new Vector3(lis.ladderRailBottom.x, 
                    gameObject.transform.position.y, lis.ladderRailBottom.z);
            }
            gameObject.transform.position = railStart;
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
            gameObject.transform.Translate(Vector3.up * (movement * _climbSpeed * Time.deltaTime));
            if (gameObject.transform.position.y < lis.ladderRailBottom.y)
            {
                gameObject.transform.position = lis.ladderRailBottom;
            } else if (gameObject.transform.position.y >= lis.ladderRailTop.y)
            {
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
        Vector3 movement = new Vector3(_move.x * _speed * Time.fixedDeltaTime, 0, _move.y * _speed * Time.fixedDeltaTime);
        movement = Quaternion.Euler(0, _cameraLocker.transform.rotation.eulerAngles.y, 0) * movement;
        
        // We don't care about velocity if the player is falling
        Vector3 velocityWithoutY = new Vector3(_body.velocity.x, 0, _body.velocity.z);

        if (velocityWithoutY.magnitude <= _maxVelocity)
        {
            _body.AddForce(movement);
        }
    }

    private bool IsGrounded()
    {
        Vector3 position = gameObject.transform.position;
        Vector3 direction = Vector3.down;
        float distance = _playerHeight / 2.0f;

        // See if ground is directly below the player
        if (Physics.Raycast(position, direction, distance))
        {
            Debug.DrawRay(position, direction);
            return true;
        }

        // See if the ground is below points in the north/south/east/west parts of the player
        else if (Physics.Raycast(position + (Vector3.forward * .5f * _playerWidth), direction, distance))
        {
            return true;
        }
        else if (Physics.Raycast(position + (Vector3.back * .5f * _playerWidth), direction, distance))
        {
            return true;
        }
        else if (Physics.Raycast(position + (Vector3.left * .5f * _playerWidth), direction, distance))
        {
            return true;
        }
        else if (Physics.Raycast(position + (Vector3.right * .5f * _playerWidth), direction, distance))
        {
            return true;
        }

        //Check if the ground is below points in the NE/NW/SE/SW parts of the player
        else if (Physics.Raycast(position + (new Vector3(0.25f, 0, 0.25f) * _playerWidth), direction, distance))
        {
            return true;
        }
        else if (Physics.Raycast(position + (new Vector3(-0.25f, 0, 0.25f) * _playerWidth), direction, distance))
        {
            return true;
        }
        else if (Physics.Raycast(position + (new Vector3(0.25f, 0, -0.25f) * _playerWidth), direction, distance))
        {
            return true;
        }
        else if (Physics.Raycast(position + (new Vector3(-0.25f, 0, -0.25f) * _playerWidth), direction, distance))
        {
            return true;
        }

        //Check if the ground is below points in the NE/NW/SE/SW parts of the player
        else if (Physics.Raycast(position + (new Vector3(0.1f, 0, 0.1f) * _playerWidth), direction, distance))
        {
            return true;
        }
        else if (Physics.Raycast(position + (new Vector3(-0.1f, 0, 0.1f) * _playerWidth), direction, distance))
        {
            return true;
        }
        else if (Physics.Raycast(position + (new Vector3(0.1f, 0, -0.1f) * _playerWidth), direction, distance))
        {
            return true;
        }
        else if (Physics.Raycast(position + (new Vector3(-0.1f, 0, -0.1f) * _playerWidth), direction, distance))
        {
            return true;
        }

        // See if the ground is below the north/south/east/west points of the player
        else if (Physics.Raycast(position + (Vector3.forward * _playerWidth), direction, distance))
        {
            return true;
        }
        else if (Physics.Raycast(position + (Vector3.back * _playerWidth), direction, distance))
        {
            return true;
        }
        else if (Physics.Raycast(position + (Vector3.left * _playerWidth), direction, distance))
        {
            return true;
        }
        else if (Physics.Raycast(position + (Vector3.right * _playerWidth), direction, distance))
        {
            return true;
        }

        //Check if the ground is below the NE/NW/SE/SW points of the player
        else if (Physics.Raycast(position + (new Vector3(0.7071f, 0, 0.7071f) * _playerWidth), direction, distance))
        {
            return true;
        }
        else if (Physics.Raycast(position + (new Vector3(-0.7071f, 0, 0.7071f) * _playerWidth), direction, distance))
        {
            return true;
        }
        else if (Physics.Raycast(position + (new Vector3(0.7071f, 0, -0.7071f) * _playerWidth), direction, distance))
        {
            return true;
        }
        else if (Physics.Raycast(position + (new Vector3(-0.7071f, 0, -0.7071f) * _playerWidth), direction, distance))
        {
            return true;
        }

        //Guess there's no ground below the player... probs...
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
}
