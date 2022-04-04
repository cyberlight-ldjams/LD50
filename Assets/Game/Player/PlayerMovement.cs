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
        float result = GetAngleToCamera();

        try { 
            movement = Quaternion.Euler(0, result, 0) * movement;
        } catch (System.Exception e)
        {
            // check if the camera is north or south
            Vector2 playerPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
            Vector2 cameraPos = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.z);

            if (playerPos.x < cameraPos.x)
            {
                //movement = Quaternion.Euler(0, 270.00001f, 0) * movement;
            } else
            {
                //movement = Quaternion.Euler(0, 90.00001f, 0) * movement;
            }
        }
        
        // We don't care about velocity if the player is falling
        Vector3 velocityWithoutY = new Vector3(_body.velocity.x, 0, _body.velocity.z);

        if (velocityWithoutY.magnitude <= _maxVelocity)
        {
            _body.AddForce(movement);
        }
    }

    // Gets the angle to the camera from the player relative to player north
    private float GetAngleToCamera()
    {
        Vector2 north = new Vector2(100000f, gameObject.transform.position.z);
        Vector2 playerPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
        Vector2 cameraPos = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.z);

        float offset = 90f;
        if (playerPos.y < cameraPos.y)
        {
            offset = offset * -1;
            north = new Vector2(north.x * -1, north.y * -1);
            if (playerPos.x < cameraPos.x)
            {
                //north = new Vector2(north.x * -1, north.y * -1);
            }
        }

        float pToN = Vector2.Distance(playerPos, north);
        float pToC = Vector2.Distance(playerPos, cameraPos);
        float cToN = Vector2.Distance(cameraPos, north);

        //Debug.Log("a2+b2=" + (Mathf.Pow(pToN, 2) + Mathf.Pow(pToC, 2)));
        //Debug.Log("a2+b2-c2=" + ((Mathf.Pow(pToN, 2) + Mathf.Pow(pToC, 2)) - Mathf.Pow(cToN, 2)));
        //Debug.Log("2ab=" + (2 * pToN * pToC));
        float res = ((Mathf.Pow(pToN, 2) + Mathf.Pow(pToC, 2)) - Mathf.Pow(cToN, 2)) / (2 * pToN * pToC);
        //Debug.Log(res);
        float ans = 0;
        if (res != -1f)
        {
            ans = Mathf.Acos(res);
        }

        float deg = ((180f / Mathf.PI) * ans) - offset;
        //Debug.Log(deg);
        if (float.IsNaN(deg))
        {
            if (playerPos.x < cameraPos.x)
            {
                return 270.0f;
            }
            else
            {
                return 90.0f;
            }
        }

        return deg;
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
